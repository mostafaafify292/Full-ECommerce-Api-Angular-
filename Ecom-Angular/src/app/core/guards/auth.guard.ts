import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { IdentityService } from '../../identity/identity.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private identityService: IdentityService, private router: Router) {}

  canActivate(): Observable<boolean> {
    return this.identityService.isAuthenticated().pipe(
      map(() => true),
      catchError(() => {
        this.router.navigate(['/account/Login'], {
          queryParams: { returnUrl: '/checkout' }
        });
        return of(false);
      })
    );
  }
}
