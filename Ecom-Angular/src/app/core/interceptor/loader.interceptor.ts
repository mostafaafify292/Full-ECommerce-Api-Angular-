import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { delay, finalize, Observable } from 'rxjs';
import { LoadingService } from '../Services/loading.service';

@Injectable()
export class loaderInterceptor implements HttpInterceptor {
  constructor(private _service: LoadingService) {}
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    this._service.loading();

    return next.handle(request).pipe(
      delay(500),
      finalize(() => {
        this._service.hideLoader();
      }),
    );
  }
}
