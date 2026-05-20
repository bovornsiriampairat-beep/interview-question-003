import { ApplicationConfig, APP_INITIALIZER } from '@angular/core';
import { provideHttpClient, HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

export class AppConfigService {
  static settings: { apiUrl: string } = { apiUrl: '' };
}

function initializeApp(http: HttpClient) {
  return () => http.get<{ apiUrl: string }>('assets/config.json').pipe(
    tap(config => AppConfigService.settings = config)
  );
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [HttpClient],
      multi: true
    }
  ]
};