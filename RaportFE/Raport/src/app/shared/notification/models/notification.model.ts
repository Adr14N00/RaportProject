export class Notification {
  message?: string;
  type?: 'info' | 'success' | 'warning' | 'error' | 'validation';

  constructor(params: Notification = {}) {
    this.message = params.message;

    if(params.type){
      this.type = params.type;
    }else{
      this.type = 'info';
    }

  }
}
