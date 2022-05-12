import { Injectable, Injector } from "@angular/core";
import { Router } from "@angular/router";
import {
  HttpClient,
  HttpHandler, HttpEvent, HttpInterceptor,
  HttpRequest, HttpResponse, HttpErrorResponse
} from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable } from "rxjs/Observable";

@Injectable()
export class AuthResponseInterceptor implements HttpInterceptor {

  currentRequest: HttpRequest<any>;
  auth: AuthService;

  constructor(private injector: Injector, private router: Router) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler): Observable<HttpEvent<any>> {

    this.auth = this.injector.get(AuthService);
    var token = (this.auth.isLoggedIn()) ? this.auth.getAuth()!.token : null;

    if (token) {
      //zapamietaj aktualne żądanie
      this.currentRequest = request;

      return next.handle(request)
        .do((event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
            //nic nie rób
          }
        })
        .catch(error => {
          return this.handleError(error, next)
        });
    }
    else {
      return next.handle(request);
    }
  }

  handleError(err: any, next: HttpHandler) {
    if (err instanceof HttpErrorResponse) {
      if (err.status === 401) {
        //Token JWT mógł przestać być ważny:
        //spróbuj otrzymać nowy za pomocą tokena odświeżania
        console.log("Token nieważny. Próba odświeżania...");

        //Zapamiętaj aktualne żądanie jako poprzednie
        var previousRequest = this.currentRequest;

        return this.auth.refreshToken()
          .flatMap((refreshed) => {
            var token = (this.auth.isLoggedIn()) ?
              this.auth.getAuth()!.token : null;
            if (token) {
              previousRequest = previousRequest.clone({
                setHeaders: { Authorization: `Bearer ${token}` }
              });
              console.log("Reset tokena z nagłówka");
            }
            return next.handle(previousRequest);
          });
      }
    }
    return Observable.throw(err);
  }
}
