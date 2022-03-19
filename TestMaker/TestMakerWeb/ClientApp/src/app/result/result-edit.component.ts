import { Component, Inject, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "result-edit",
  templateUrl: './result-edit.component.html',
  styleUrls: ['./result-edit.component.css']
})

export class ResultEditComponent {
  result: Result;
  title: string;
  //TRUE jeśli istnieje, FALSE jeśli nowy wynik
  editMode: boolean;

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private activatedRoute: ActivatedRoute,
    private router: Router) {

    //Utwórz pusty obiekt na podstawie interfejsu Result
    this.result = <Result>{};
    var id = +this.activatedRoute.snapshot.params["id"];

    //Sprawdź, czy jestesmy w trybie edycji
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //pobierz pytanie z serwera
      var url = this.baseUrl + "api/result/" + id;
      this.http.get<Result>(url).subscribe(result => {
        this.result = result;
        this.title = "Edycja - " + this.result.Text;
      }, error => console.error(error));
    }
    else {
      this.result.QuizId = id;
      this.title = "Utwórz nowy wynik";
    }
  }

  onSubmit(result: Result) {
    var url = this.baseUrl + "api/result";

    if (this.editMode) {
      this.http.put<Result>(url, result).subscribe(res => {
        var v = res;
        console.log("Wynik " + v.Id + " został uaktualniony.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.error(error));
    }
    else {
      this.http.post<Result>(url, result).subscribe(res => {
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
