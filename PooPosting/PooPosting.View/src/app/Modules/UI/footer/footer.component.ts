import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
  @Input() appTitle!: string;
  currentYear = new Date().getFullYear();
  constructor() { }

  ngOnInit(): void {
  }

  openLink(url: string){
    window.open(url, "_blank");
  }
}
