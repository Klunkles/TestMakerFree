﻿import { Component, Inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
    selector: "result-edit",
    templateUrl: './result-edit.component.html',
    styleUrls: ['./result-edit.component.html']
})

export class ResultEditComponent {
    title: string;
    result: Result;

    //this will be TRUE when editing a result and FALSE when creating one
    editMode: boolean;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string
    ) {
        this.result = <Result>{};
        var id = +this.activatedRoute.snapshot.params["id"];

        this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
        if (this.editMode) {
            var url = this.baseUrl + "api/result/" + id;
            this.http.get<Result>(url).subscribe(r => {
                this.result = r;
                this.title = "Edit - " + this.result.Text;
            }, error => console.error(error));
        } else {
            this.result.QuizId = id;
            this.title = "Create a new Result";
        }
    }

    onSubmit(result: Result) {
        var url = this.baseUrl + "api/result";
        if (this.editMode) {
            this.http.post<Result>(url, result).subscribe(r => {
                var v = result;
                console.log("Result " + v.Id + " has been updated");
                this.router.navigate(["quiz/edit", v.QuizId]);
            }, error => console.log(error));
        } else {
            this.http.put<Result>(url, result).subscribe(r => {
                var v = r;
                console.log("Result " + v.Id + " has been created");
                this.router.navigate(["quiz/edit", v.QuizId]);
            }, error => console.log(error));
        }
    }

    onBack() {
        this.router.navigate(["quiz/edit", this.result.QuizId]);
    }
}