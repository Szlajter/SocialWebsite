import { Component } from '@angular/core';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  isCollapsed: boolean = true;

  constructor(public accountService: AccountService) {}

  logout(){
    this.accountService.logout();
  }

  toggleCollapse() {
    this.isCollapsed = !this.isCollapsed;
  }
}
