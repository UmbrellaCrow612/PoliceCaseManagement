import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';
import { HttpErrorResponse } from '@angular/common/http';
import { appPaths } from '../../../../core/app/constants/appPaths';
import CODES from '../../../../core/server-responses/codes';
/**
 * This acts as a boiler page - we use it as a page when a user clicks the link from the email with domain/confirm-email-code?code=123
 * they will get directed here then we simply take the code from the URL and make a request to confirm endpoint
 * if it fails - show error message why - if successful then direct to login
 */
@Component({
  selector: 'app-confirm-email-with-code-view',
  imports: [RouterLink],
  templateUrl: './confirm-email-with-code-view.component.html',
  styleUrl: './confirm-email-with-code-view.component.css',
})
export class ConfirmEmailWithCodeViewComponent implements OnInit {
  code: string | null = null;
  email: string | null = null;
  errorMessage: string | null = null;
  loginUrl: string = `../${appPaths.LOGIN}`;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe({
      next: (res) => {
        let _code = res['code']; // fetched from ?code=123 named "code"
        if (_code) {
          this.code = _code;
        }
        let _email = res['email']; // fetched from ?email named "email"
        if (_email) {
          this.email = _email;
        }
      },
    });
    this.sendConfirmationEmailCode();
  }

  /**
   * Sends the code to backend to the API
   */
  sendConfirmationEmailCode() {
    if (this.code && this.email) {
      this.authService
        .SendConfirmationEmailCode(this.email, this.code)
        .subscribe({
          next: () => {
            this.router.navigate([`../${appPaths.LOGIN}`], {
              relativeTo: this.route,
            });
          },
          error: (err: HttpErrorResponse) => {
            let code = err.error[0]?.code;

            if (code == CODES.EMAIL_ALREADY_CONFIRMED) {
              this.router.navigate([`../${appPaths.LOGIN}`], {
                relativeTo: this.route,
              });
              return;
            }

            if (code == CODES.USER_DOES_NOT_EXIST || CODES.EMAIL_CONFIRMATION) {
              this.errorMessage = 'Invalid confirmation attempt';
              return;
            }

            this.errorMessage = `Unknown error message status: ${
              err.status
            } error: ${JSON.stringify(err.error)}`;
          },
        });
    } else {
      this.errorMessage = `Invalid URL format missing ${
        this.code ? '' : 'code'
      } ${this.email ? '' : 'email'} from URL params`;
    }
  }
}
