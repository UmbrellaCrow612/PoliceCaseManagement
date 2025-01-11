import { Injectable } from "@angular/core";
import { AuthenticationService } from "../../authentication/services/authentication.service";

@Injectable({
    providedIn: 'root',
})
export class UserService {

    constructor(private authService: AuthenticationService){}

    GetCurrentUserDetails(){
       var tok = this.authService.GetJwtToken();
       var dec = this.authService.DecodeJwtToken(tok);

       return this.CreateUserObjectFromJwtTokenDetails(dec)
    }

    private CreateUserObjectFromJwtTokenDetails(tokenDetails:any){

    }
}