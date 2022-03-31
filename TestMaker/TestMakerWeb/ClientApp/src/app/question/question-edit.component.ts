import { Component, Inject, OnInit } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms"
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "question-edit",
  templateUrl: './question-edit.component.html',
  styleUrls: ['./question-edit.component.css']
})

export class QuestionEditComponent {
  question: Question;
  title: string;
  form: FormGroup;
  activityLog: string;
  //TRUE jeśli istnieje, FALSE jeśli nowe pytanie
  editMode: boolean;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router) {

    //Utwórz pusty obiekt na podstawie interfejsu Question
    this.question = <Question>{};
    var id = +this.activatedRoute.snapshot.params["id"];

    this.createForm();

    //Sprawdź, czy jestesmy w trybie edycji
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //pobierz pytanie z serwera
      var url = this.baseUrl + "api/question/" + id;
      this.http.get<Question>(url).subscribe(result => {
        this.question = result;
        this.title = "Edycja - " + this.question.Text;
        this.updateForm();
      }, error => console.error(error));
    }
    else {
      this.question.QuizId = id;
      this.title = "Utwórz nowe pytanie";
    }
  }

  createForm() {
    this.form = this.fb.group({
      Text: ['', Validators.required]
    });

    this.activityLog = '';
    this.log("Formularz został zainicjalizowany.");

    //Reaguj na zmiany w formularzu
    this.form.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("Model formularza wczytany.");
        }
        else {
          this.log("Formularz zaktualizowany przez użytkownika.");
        }
      })

    //Reaguj na zmiany w kontrolce form.Text
    this.form.get("Text")!.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("Kontrolka Text wczytana z wartością domyślną.");
        }
        else {
          this.log("Kontrolka Text uaktualniona przez użytkownika.");
        }
      })
  }

  log(str: string) {
    this.activityLog += "[" + new Date().toLocaleString() + "] " + str + "<br/>";
  }

  updateForm() {
    this.form.setValue({
      Text: this.question.Text
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
    var tempQuestion = <Question>{};
    tempQuestion.Text = this.form.value.Text;
    tempQuestion.QuizId = this.question.QuizId;

    var url = this.baseUrl + "api/question";

    if (this.editMode) {
      tempQuestion.Id = this.question.Id;
      this.http.put<Question>(url, tempQuestion).subscribe(res => {
        var v = res;
        console.log("Pytanie " + v.Id + " zostało uaktualnione.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.error(error));
    }
    else {
      this.http.post<Question>(url, tempQuestion).subscribe(res => {
        var v = res;
        console.log("Pytanie " + v.Id + " zostało utworzone.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.error(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.question.QuizId]);
  }
}
