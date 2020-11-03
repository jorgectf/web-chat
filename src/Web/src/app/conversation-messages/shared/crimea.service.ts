import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import {catchError, takeUntil} from 'rxjs/operators';

import {AbstractApiService} from '../../core/abstract-api.service';
import {ConversationMessageModel} from './conversation-message.model';
import {ApiEndpoints} from '../../shared/api.constants';

@Injectable()
export class CrimeaService extends AbstractApiService<ConversationMessageModel> {
  constructor(http: HttpClient) {
    super(http);
  }

  public getById(request: DetailsQuery, handler: OnDetails) {
    this.http.get<ConversationMessageModel>(`${ApiEndpoints.CrimeaGetById}/${request.conversationId}`)
      .pipe(catchError(this.handleError))
      .pipe(takeUntil(this.stop$))
      .subscribe(model => handler.onDetailsSuccess(model), error => handler.onDetailsFailed(error));
  }
}

///////////////////////////////////////////////////////////////////////////
// Details Query
///////////////////////////////////////////////////////////////////////////

export class DetailsQuery {
  public constructor(public conversationId: number) {
  }
}

export interface OnDetails {
  onDetailsSuccess(model: ConversationMessageModel): void;

  onDetailsFailed(error: any): void;
}
