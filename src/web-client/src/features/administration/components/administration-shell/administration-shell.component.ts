import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDrawerMode, MatSidenavModule } from '@angular/material/sidenav';
import { fromEvent, Subscription, throttleTime } from 'rxjs';
import { AdministrationSideNavService } from '../../services/administration-side-nav-service.service';
import { RouterOutlet } from '@angular/router';
import { AdministrationHeaderComponent } from '../administration-header/administration-header.component';

@Component({
  selector: 'app-administration-shell',
  imports: [MatSidenavModule, RouterOutlet, AdministrationHeaderComponent],
  templateUrl: './administration-shell.component.html',
  styleUrl: './administration-shell.component.css',
})
export class AdministrationShellComponent implements OnInit, OnDestroy {
  constructor(private adminSideBarService: AdministrationSideNavService) {}
  screenWidth: number = window.innerWidth;
  resizeSubscription: Subscription | null = null;
  isMobileView = false;
  sideNavMode: MatDrawerMode = 'over';
  isOpen = false;
  openSubscription: Subscription | null = null;
  isMobileSubscription: Subscription | null = null;

  changeSideNav() {
    this.screenWidth = window.innerWidth;
    if (this.screenWidth < 800) {
      this.adminSideBarService.toggleMobile(true);
    } else {
      this.adminSideBarService.toggleMobile(false);
    }

    if (this.isMobileView) {
      this.sideNavMode = 'over';
    } else {
      this.sideNavMode = 'side';
    }
  }

  ngOnInit(): void {
    this.resizeSubscription = fromEvent(window, 'resize')
      .pipe(throttleTime(200))
      .subscribe(() => {
        this.changeSideNav();
      });
    this.openSubscription = this.adminSideBarService.isOpenSubject.subscribe({
      next: (val) => {
        this.isOpen = val;
      },
    });
    this.isMobileSubscription =
      this.adminSideBarService.isMobileSubject.subscribe({
        next: (val) => {
          this.isMobileView = val;
        },
      });
    this.changeSideNav();
  }

  toggle() {
    if (this.isOpen) {
      this.adminSideBarService.toggle(false);
    } else {
      this.adminSideBarService.toggle(true);
    }
  }

  ngOnDestroy(): void {
    if (this.resizeSubscription) {
      this.resizeSubscription.unsubscribe();
    }
    if (this.openSubscription) {
      this.openSubscription.unsubscribe();
    }
    if (this.isMobileSubscription) {
      this.isMobileSubscription.unsubscribe();
    }
  }
}
