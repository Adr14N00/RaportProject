import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard = () => {

  const router = inject(Router);
  const authService = inject(AuthService);

  if(authService.isLoggedIn()) return true;

  return router.parseUrl('/login');
};
