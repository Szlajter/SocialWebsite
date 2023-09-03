import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';
import { AccountService } from '../services/account.service';

export const adminGuard = () => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map(user => {
      if(!user) {
        return false;
      }
      if(user.roles.includes("Admin") || user.roles.includes("Moderator")) {
         return true;
      }
      else {
        toastr.error("Unauthorized");
        return false;
      }
    })
  )
}

