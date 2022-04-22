import { Component, Inject } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

@Component({
  selector: "login",
  templateUrl: "./login.component.html"
})

export class LoginComponent {
  title: string;
  form: FormGroup;

  constructor(private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    @Inject('BASE_URL') private baseUrl: string) {

    this.title = "Zaloguj się";

    //Inicjalizacja formularza
    this.createForm();
  }

  createForm() {
    this.form = this.fb.group({
      Username: ['', Validators.required],
      Password: ['', Validators.required]
    })
  }

  onSubmit() {
    var url = this.baseUrl + "api/token/auth";
    var username = this.form.value.Username;
    var password = this.form.value.Password;

    this.authService.login(username, password).subscribe(res => {
      //Logowanie udane
      //Wyświetl dane logowania w okienku alert.
      //WAŻNE: usuń po zakończeniu testu
      alert("Logowanie udane "
        + "NAZWA UŻYTKOWNIKA: " + username
        + " TOKEN: " + this.authService.getAuth()!.token);

      this.router.navigate(["home"]);
    },
      err => {
        //Logowanie nieudane
        console.log(err);
        this.form.setErrors({
          "auth": "Niepoprawna nazwa użytkownika lub hasło"
        });
      });
  }

  onBack() {
    this.router.navigate(["home"]);
  }

  //Pobierz FormControl
  getFormControl(name: string) {
    return this.form.get(name);
  }

  //Zwróć TRUE, jeśli element FormControl jest poprawny
  isValid(name: string) {
    var e = this.getFormControl(name);
    return e && e.valid;
  }

  //Zwróć TRUE, jeśli element FormControl uległ zmianie
  isChanged(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched);
  }

  //Zwróć TRUE, jeśli element FormControl nie jest poprawny po wprowadzeniu zmian
  hasError(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }
}
