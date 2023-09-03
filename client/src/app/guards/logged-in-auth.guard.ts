import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';
import { AccountService } from '../services/account.service';

export const loggedInGuard = () => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map(user => {
      if(user) {
        toastr.error("Already sign in");
        return false;
      }
      else{
         return true;
      }
    })
  )
}

