import { EventEmitter, Inject, Injectable, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class AuthService {
  authKey: string = "auth";
  clientId: string = "TestMaker";

  constructor(private http: HttpClient,
    @Inject('PLATFORM_ID') private platformId: any) {
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

    return this.http.post<TokenResponse>(url, data).map((res) => {
      let token = res && res.token;
      //jeśli otrzymaliśmy token, logowanie powiodło się
      if (token) {
        //Zapamiętaj nazwę użytkownika i token JWT
        this.setAuth(res);
        //Logowanie udane
        return true
      }
      //Logowanie nieudane
      return Observable.throw('Unauthorized');
    })
      .catch(error => {
        return new Observable<any>(error);
      });
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
