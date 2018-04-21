import { Component, Input, Inject, OnChanges, SimpleChanges } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";

@Component({
    selector: "result-list",
    templateUrl: './result-list.component.html',
    styleUrls: ['./result-list.component.css']
})

export class ResultListComponent {
    @Input() quiz: Quiz;
    title: string;
    results: Result[];

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        private router: Router
    ) {
        this.results = [];
    }

    ngOnChanges(changes: SimpleChanges) {
        if (typeof changes['quiz'] !== "undefined") {
            //retrieve the result variable change info
            var change = changes['quiz'];

            //only perform the task if the value has been changed
            if (!change.isFirstChange()) {
                //execute the Http request and retrieve the result
                this.loadData();
            }
        }
    }

    loadData() {
        var url = this.baseUrl + "api/result/All/" + this.quiz.Id;
        this.http.get<Result[]>(url).subscribe(result => {
            this.results = result;
        }, error => console.error(error));
    }

    onCreate() {
        this.router.navigate(["/result/create", this.quiz.Id]);
    }

    onEdit(result: Result) {
        this.router.navigate(["/result/edit", result.Id]);
    }

    onDelete(result: Result) {
        if (confirm("Do you really want to delete this result?")) {
            var url = this.baseUrl + "api/result/" + result.Id;
            this.http.delete(url, { responseType: 'text' }).subscribe(r => {
                console.log("Result " + result.Id + " has been deleted.");
                this.loadData();
            }, error => console.log(error));
        }
    }
}