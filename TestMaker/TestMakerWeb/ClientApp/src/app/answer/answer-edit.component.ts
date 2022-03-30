import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "answer-edit",
  templateUrl: './answer-edit.component.html',
  styleUrls: ['./answer-edit.component.css']
})

export class AnswerEditComponent {
  answer: Answer;
  title: string;
  form: FormGroup;
  //TRUE jeśli istnieje, FALSE jeśli nowa odpowiedź
  editMode: boolean;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router) {

    //Utwórz pusty obiekt na podstawie interfejsu Answer
    this.answer = <Answer>{};
    var id = +this.activatedRoute.snapshot.params["id"];

    this.createForm();

    //Sprawdź, czy jestesmy w trybie edycji
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //pobierz odpowiedź z serwera
      var url = this.baseUrl + "api/answer/" + id;
      this.http.get<Answer>(url).subscribe(result => {
        this.answer = result;
        this.title = "Edycja - " + this.answer.Text;
        this.updateForm();
      }, error => console.error(error));
    }
    else {
      this.answer.QuestionId = id;
      this.title = "Utwórz nową odpowiedź";
    }
  }

  createForm() {
    this.form = this.fb.group({
      Text: ['', Validators.required],
      Value: ['',
        [Validators.required, Validators.min(-5), Validators.max(5)]
      ]
    });
  }

  updateForm() {
    this.form.setValue({
      Text: this.answer.Text,
      Value: this.answer.Value
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
    var tempAnswer = <Answer>{};
    tempAnswer.Text = this.form.value.Text;
    tempAnswer.Value = this.form.value.Value;
    tempAnswer.QuestionId = this.answer.QuestionId;

    var url = this.baseUrl + "api/answer";

    if (this.editMode) {
      this.http.put<Answer>(url, tempAnswer).subscribe(res => {
        var v = res;
        console.log("Odpowiedź " + v.Id + " została uaktualniona.");
        this.router.navigate(["question/edit", v.QuestionId]);
      }, error => console.error(error));
    }
    else {
      this.http.post<Answer>(url, tempAnswer).subscribe(res => {
        var v = res;
        console.log("Odpowiedź " + v.Id + " została utworzona.");
        this.router.navigate(["question/edit", v.QuestionId]);
      }, error => console.error(error));
    }
  }

  onBack() {
    this.router.navigate(["question/edit", this.answer.QuestionId]);
  }
}
