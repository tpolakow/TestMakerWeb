import { Component, Inject, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'quiz-list',
  templateUrl: './quiz-list.component.html',
  styleUrls: ['./quiz-list.component.css']
})

export class QuizListComponent implements OnInit {
  @Input() class: string;
  title: string;
  selectedQuiz: Quiz;
  quizzes: Quiz[];
  http: HttpClient;
  baseUrl: string;

  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    console.log("QuizListComponent " + "został utworzony z użyciem następującej klasy: " + this.class);

    var url = this.baseUrl + "api/quiz/";

    switch (this.class) {
      case "latest":
      default:
        this.title = "Najnowsze";
        url += "Latest/";
        break;
      case "byTitle":
        this.title = "Alfabetycznie";
        url += "ByTitle/";
        break;
      case "random":
        this.title = "Losowe"
        url += "Random/";
        break;
    }

    this.http.get<Quiz[]>(url).subscribe(result => {
      this.quizzes = result;
    }, error => console.error(error));
  }

  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("Wybrano quiz o identyfikatorze " + this.selectedQuiz.Id);
  }
}
