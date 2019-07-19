import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; // this will store our username and password

  constructor() {}

  ngOnInit() {}

  login() {
    console.log(this.model);
  }
}
