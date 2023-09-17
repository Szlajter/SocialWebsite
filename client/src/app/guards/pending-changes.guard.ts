
import { CanDeactivateFn } from "@angular/router";
import { EditProfilePageComponent } from "../components/edit-profile-page/edit-profile-page.component";
import { Observable } from "rxjs";
import { ConfirmService } from "../services/confirm.service";
import { inject } from "@angular/core";


export const PendingChangesGuard: CanDeactivateFn<EditProfilePageComponent> = (
  component: EditProfilePageComponent
  ): Observable<boolean> | boolean => {
  const confirmService = inject(ConfirmService)

  if(component.editForm?.dirty){
    return confirmService.confirm();
  } 
  return true;
}

