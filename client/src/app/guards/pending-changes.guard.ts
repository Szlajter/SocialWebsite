
import { CanDeactivateFn } from "@angular/router";
import { EditProfilePageComponent } from "../components/edit-profile-page/edit-profile-page.component";
import { Observable } from "rxjs";


export const PendingChangesGuard: CanDeactivateFn<EditProfilePageComponent> = (
  component: EditProfilePageComponent
  ):Observable<boolean> | boolean => {
  if(component.editForm?.dirty){
    return confirm("Are you sure you want to continue? You will lose all the changes you have made.");
  } 
  return true;
}

