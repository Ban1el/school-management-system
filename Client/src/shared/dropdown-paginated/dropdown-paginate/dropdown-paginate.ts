import {
  Component,
  OnInit,
  OnDestroy,
  signal,
  input,
  output,
  forwardRef,
  ElementRef,
  inject,
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  FormControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { DropdownItem } from '../../../types/Dropdown/DropdownItemDto';

@Component({
  selector: 'app-dropdown-paginate',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './dropdown-paginate.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DropdownPaginate),
      multi: true,
    },
  ],
})
export class DropdownPaginate implements ControlValueAccessor, OnInit, OnDestroy {
  private elementRef = inject(ElementRef);

  // Inputs
  label = input('Select');
  placeholder = input('Search...');
  legendLabel = input('Name');
  items = input<DropdownItem[]>([]);
  loading = input(false);

  // Outputs
  itemSelected = output<DropdownItem | null>();
  searchChanged = output<string>();
  scrolledToBottom = output<void>();

  selectedItem: DropdownItem | null = null;
  isOpen = signal(false);
  searchControl = new FormControl('');

  private destroy$ = new Subject<void>();

  private onChange: (val: DropdownItem | null) => void = () => {};
  private onTouched: () => void = () => {};

  onOutsideClick = (event: MouseEvent) => {
    if (!this.elementRef.nativeElement.contains(event.target as Node)) {
      this.isOpen.set(false);
    }
  };

  ngOnInit() {
    document.addEventListener('click', this.onOutsideClick);
    this.selectedItem = { id: 0, name: this.label() };

    this.searchControl.valueChanges
      .pipe(debounceTime(300), takeUntil(this.destroy$))
      .subscribe((val) => this.searchChanged.emit(val ?? ''));
  }

  ngOnDestroy() {
    document.removeEventListener('click', this.onOutsideClick);
    this.destroy$.next();
    this.destroy$.complete();
  }

  onScroll(event: Event) {
    const el = event.target as HTMLElement;
    if (el.scrollHeight - el.scrollTop <= el.clientHeight + 5) {
      this.scrolledToBottom.emit();
    }
  }

  toggleDropdown(event: MouseEvent) {
    event.stopPropagation();
    this.isOpen.set(!this.isOpen());
  }

  selectItem(item: DropdownItem | null, event: MouseEvent) {
    event.stopPropagation();
    this.selectedItem = item ?? { id: 0, name: this.label() };
    this.onChange(this.selectedItem);
    this.onTouched();
    this.itemSelected.emit(this.selectedItem);
    this.isOpen.set(false);
  }

  writeValue(val: DropdownItem | null) {
    this.selectedItem = val ?? { id: 0, name: this.label() };
  }

  registerOnChange(fn: (val: DropdownItem | null) => void) {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void) {
    this.onTouched = fn;
  }
}
