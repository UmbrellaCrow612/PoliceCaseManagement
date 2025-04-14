import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { AdministrationSideNavService } from '../../services/administration-side-nav-service.service';
import { Subscription } from 'rxjs';
import { HeaderProfileComponent } from "../../../../core/components/header-profile/header-profile.component";

@Component({
  selector: 'app-administration-header',
  imports: [MatIcon, MatButtonModule, HeaderProfileComponent],
  templateUrl: './administration-header.component.html',
  styleUrl: './administration-header.component.css',
})
export class AdministrationHeaderComponent implements OnInit, OnDestroy {
  constructor(private sideBarService: AdministrationSideNavService) {}

  isOpen = false;
  isOpenSub: Subscription | null = null;

  ngOnInit(): void {
    this.isOpenSub = this.sideBarService.isOpenSubject.subscribe({
      next: (val) => {
        this.isOpen = val;
      },
    });
  }

  toggle() {
    this.sideBarService.toggle(!this.isOpen);
  }

  ngOnDestroy(): void {
    if (this.isOpenSub) {
      this.isOpenSub.unsubscribe();
    }
  }
}
