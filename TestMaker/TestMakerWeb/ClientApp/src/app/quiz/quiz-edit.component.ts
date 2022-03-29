import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'quiz-edit',
  templateUrl: './quiz-edit.component.html',
  styleUrls: ['./quiz-edit.component.css']
})

export class QuizEditComponent {
  title: string;
  quiz: Quiz;
  form: FormGroup;
  //TRUE jesli edycja istniejacego quizu
  //FALSE jesli nowy quiz
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private fb: FormBuilder,
    @Inject('BASE_URL') private baseUrl: string  ) {

    //Utwórz pusty obiekt zgodny z interfejsem Quiz
    this.quiz = <Quiz>{};

    //Zainicjalizuj formularz
    this.createForm();

    var id = +this.activatedRoute.snapshot.params["id"];
    if (id) {
      this.editMode = true;

      //Pobierz quiz z serwera
      var url = this.baseUrl + "api/quiz/" + id;
      this.http.get<Quiz>(url).subscribe(result => {
        this.quiz = result;
        this.title = "Edycja - " + this.quiz.Title;
        //Uaktualnij formularz wartością z quizu
        this.updateForm();
      }, error => console.error(error));
    }
    else {
      this.editMode = false;
      this.title = "Utwórz nowy quiz";
    }
  }

  createForm() {
    this.form = this.fb.group({
      Title: ['', Validators.required],
      Description: '',
      Text: ''
    });
  }

  updateForm() {
    this.form.setValue({
      Title: this.quiz.Title,
      Description: this.quiz.Description || '',
      Text: this.quiz.Text || ''
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
    //Zbuduj tymczasowy obiekt quizu na podstawie wartości z formularza
    var tempQuiz = <Quiz>{};
    tempQuiz.Title = this.form.value.Title;
    tempQuiz.Description = this.form.value.Description;
    tempQuiz.Text = this.form.value.Text;

    var url = this.baseUrl + "api/quiz";

    if (this.editMode) {
      tempQuiz.Id = this.quiz.Id;

      this.http.put<Quiz>(url, tempQuiz).subscribe(result => {
        var v = result;
        console.log("Quiz " + v.Id + " został uaktualniony.");
        this.router.navigate(["home"]);
      }, error => console.log(error));
    }
    else {
      this.http.post<Quiz>(url, tempQuiz).subscribe(result => {
        var v = result;
        console.log("Quiz " + v.Id + " został utworzony.");
        this.router.navigate(["home"]);
      }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["home"]);
  }
}
