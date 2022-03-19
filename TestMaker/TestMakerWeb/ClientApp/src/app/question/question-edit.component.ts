import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "question-edit",
  templateUrl: './question-edit.component.html',
  styleUrls: ['./question-edit.component.css']
})

export class QuestionEditComponent implements OnInit {
  question: Question;
  title: string;
  //TRUE jeśli istnieje, FALSE jeśli nowe pytanie
  editMode: boolean;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private activatedRoute: ActivatedRoute,
    private router: Router) {

    //Utwórz pusty obiekt na podstawie interfejsu Question
    this.question = <Question>{};
    var id = +this.activatedRoute.snapshot.params["id"];

    //Sprawdź, czy jestesmy w trybie edycji
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //pobierz pytanie z serwera
      var url = this.baseUrl + "api/question/" + id;
      this.http.get<Question>(url).subscribe(result => {
        this.question = result;
        this.title = "Edycja - " + this.question.Text;
      }, error => console.error(error));
    }
    else {
      this.question.QuizId = id;
      this.title = "Utwórz nowe pytanie";
    }
  }

  onSubmit(question: Question) {
    var url = this.baseUrl + "api/question";

    if (this.editMode) {
      this.http.put<Question>(url, question).subscribe(res => {
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
