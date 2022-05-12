import { Component, Inject } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "register",
  templateUrl: './register.component.html'
})

export class RegisterComponent {
  title: string;
  form: FormGroup;

  constructor(private router: Router,
    private fb: FormBuilder,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {

    this.title = "Rejestracja nowego użytkownika";

    //Inicjalizacja formularza
    this.createForm();
  }

  createForm() {
    this.form = this.fb.group({
      Username: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', Validators.required],
      PasswordConfirm: ['', Validators.required],
      DisplayName: ['', Validators.required]
    }, {
      validator: this.passwordConfirmValidator
    });
  }

  onSubmit() {
    //Zbuduj tymczasowy obiekt z wartości z formularza
    var tempUser = <User>{};
    tempUser.UserName = this.form.value.Username;
    tempUser.Email = this.form.value.Email;
    tempUser.Password = this.form.value.Password;
    tempUser.DisplayName = this.form.value.DisplayName;

    var url = this.baseUrl + "api/user";

    this.http.post<User>(url, tempUser).subscribe(res => {
      if (res) {
        var v = res;
        console.log("Użytkownik " + v.UserName + " został utworzony");
        //przejdz do strony logowania
        this.router.navigate(["login"]);
      } else {
        //Rejestracja nieudana
        this.form.setErrors({
          "register": "Rejestracja nie powiodła się"
        });
      }
    }, error => console.log(error));
  }

  onBack() {
    this.router.navigate(["home"]);
  }

  passwordConfirmValidator(control: FormControl): any {
    let p = control.root.get('Password');
    let pc = control.root.get('PasswordConfirm');

    if (p && pc) {
      if (p.value !== pc.value) {
        pc.setErrors({
          "PasswordMismatch": true
        });
      }
      else {
        pc.setErrors(null);
      }
    }
    return null;
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
