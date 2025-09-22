import { AuthInterceptor } from './auth.interceptor';
import { AuthService } from './services/auth.service';

describe('AuthInterceptor', () => {
  let authService: AuthService;
  let interceptor: AuthInterceptor;

  beforeEach(() => {
    authService = {} as AuthService; // or a proper mock
    interceptor = new AuthInterceptor(authService); // ✅ must use new
  });

  it('should be created', () => {
    expect(interceptor).toBeTruthy();
  });
});
