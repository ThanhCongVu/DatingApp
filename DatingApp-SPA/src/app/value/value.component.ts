import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any;
  // declare property "value" with type any
  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(
      // to view the Observable object, we first need to subscribe to it
      response => {
        this.values = response;
      },
      error => {
        console.log(error);
      }
    );
  }
}
