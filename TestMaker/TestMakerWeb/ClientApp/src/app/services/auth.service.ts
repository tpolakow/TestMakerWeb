import { EventEmitter, Inject, Injectable, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import 'rxjs/Rx';
import 'rxjs/add/operator/map'
import { catchError, map } from 'rxjs/operators'

@Injectable()
export class AuthService {
  authKey: string = "auth";
  clientId: string = "TestMaker";

  constructor(private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: any) {
  }

  login(username: string, password: string): Observable<boolean> {
    var url = "api/token/auth";
    var data = {
      username: username,
      password: password,
      client_id: this.clientId,
      //Wymagane do zalogowania się przy użyciu nazwy użytkownika i hasła
      grant_type: "password",
      //Oddzielona spacjami lista zakresów, dla których token będzie ważny
      scope: "offline_access profile email"
    };

    return this.getAuthFromServer(url, data);
  }

  //Spróbuj odświerzyć token
  refreshToken(): Observable<boolean> {
    var url = "api/token/auth";
    var data = {
      client_id: this.clientId,
      //Wymagane do zalogowania sie przy uzyciu nazwy uzytkownika i hasla
      grant_type: "refresh_token",
      refresh_token: this.getAuth()!.refresh_token,
      //Oddzielona spacjami lista zakresów, dla których token będzie ważny
      scope: "offline_access profile email"
    };

    return this.getAuthFromServer(url, data);
  }

  getAuthFromServer(url: string, data: any): Observable<boolean> {
    return this.http.post<TokenResponse>(url, data).map((res) => {
      let token = res && res.token;
      //Jeśli jest token, logowanie sie udalo
      if (token) {
        //zapamietaj nazwe uzytkownika i tokeny
        this.setAuth(res);
        //logowanie udane
        return true;
      }
      //logowanie nieudane
      return Observable.throw('Unauthorized');
    })
      .catch(error => {
        return new Observable<any>(error);
      })
  }

  logout(): boolean {
    this.setAuth(null);
    return true;
  }

  //Umieść dane o uwierzytelnieniu w localStorage lub usuń dane, jeśli przekazano NULL
  setAuth(auth: TokenResponse | null): boolean {
    if (isPlatformBrowser(this.platformId)) {
      if (auth) {
        localStorage.setItem(this.authKey, JSON.stringify(auth));
      }
      else {
        localStorage.removeItem(this.authKey);
      }
    }
    return true;
  }

  //Pobiera obiekt z danymi uwierzytelnienia (lub zwraca NULL, jeśli nie istnieje)
  getAuth(): TokenResponse | null {
    if (isPlatformBrowser(this.platformId)) {
      var i = localStorage.getItem(this.authKey);
      if (i) {
        return JSON.parse(i);
      }
    }
    return null;
  }

  //Zwraca TRUE, jeśli użytkownik jest zalogowany, lub FALSE w sytuacji przeciwnej
  isLoggedIn(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.authKey) != null;
    }
    return false;
  }
}
