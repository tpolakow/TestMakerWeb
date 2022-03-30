import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "result-edit",
  templateUrl: './result-edit.component.html',
  styleUrls: ['./result-edit.component.css']
})

export class ResultEditComponent {
  result: Result;
  title: string;
  form: FormGroup;
  //TRUE jeśli istnieje, FALSE jeśli nowy wynik
  editMode: boolean;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router) {

    //Utwórz pusty obiekt na podstawie interfejsu Result
    this.result = <Result>{};
    var id = +this.activatedRoute.snapshot.params["id"];

    this.createForm();

    //Sprawdź, czy jestesmy w trybie edycji
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //pobierz pytanie z serwera
      var url = this.baseUrl + "api/result/" + id;
      this.http.get<Result>(url).subscribe(result => {
        this.result = result;
        this.title = "Edycja - " + this.result.Text;
        this.updateForm();
      }, error => console.error(error));
    }
    else {
      this.result.QuizId = id;
      this.title = "Utwórz nowy wynik";
    }
  }

  createForm() {
    this.form = this.fb.group({
      Text: ['', Validators.required],
      MinValue: ['', Validators.pattern("^\\d*$")],
      MaxValue: ['', Validators.pattern(/^\d*$/)]
    });
  }

  updateForm() {
    this.form.setValue({
      Text: this.result.Text,
      MinValue: this.result.MinValue,
      MaxValue: this.result.MaxValue
    });
  }

  getFormControl(name: string) {
    return this.form.get(name);
  }

  isValid(name: string) {
    var e = this.getFormControl(name);
    return e && e.valid;
  }

  isChanged(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched)
  }

  hasError(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }

  onSubmit() {
    var tempResult = <Result>{};
    tempResult.Text = this.form.value.Text;
    tempResult.MinValue = this.form.value.MinValue;
    tempResult.MaxValue = this.form.value.MaxValue;
    tempResult.QuizId = this.result.QuizId;

    var url = this.baseUrl + "api/result";

    if (this.editMode) {
      this.http.put<Result>(url, tempResult).subscribe(res => {
        var v = res;
        console.log("Wynik " + v.Id + " został uaktualniony.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.error(error));
    }
    else {
      this.http.post<Result>(url, tempResult).subscribe(res => {
        var v = res;
        console.log("Wynik " + v.Id + " został utworzony.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.error(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.result.QuizId]);
  }
}
