export class ResultNotification {
  message?: string;
  timeout?: number;
  type?: 'success' = 'success';
  position?: 'bottom-right' | 'top-right' = 'top-right';

  constructor(params: ResultNotification = {}) {
    this.message = params.message;

    if(params.timeout){
      this.timeout = params.timeout
    }else{
      this.timeout = 3;
    }
    if(params.type){
      this.type = params.type;
    }else{
      this.type = 'success';
    }
    if(params.position){
      this.position = params.position;
    }else{
      this.position = 'top-right';
    }

  }
}
