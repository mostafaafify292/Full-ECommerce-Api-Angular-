import {
  Component,
  EventEmitter,
  Input,
  input,
  Output,
  output,
} from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
})
export class PaginationComponent {
  @Input() TotalCount: number;
  @Output() pageChanged = new EventEmitter();

  OnChangePage(ev: any) {
    this.pageChanged.emit(ev);
  }
}
