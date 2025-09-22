import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StoreService } from '../../../store.service';
import { checkApi } from '../../../works';
import { getApi } from '../../../api';
import { ReadFileInfo } from '../../../types';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-docker-compose-view',
  imports: [JsonPipe],
  templateUrl: './docker-compose-view.component.html',
  styleUrl: './docker-compose-view.component.css',
})
export class DockerComposeViewComponent implements OnInit {
  router = inject(Router);
  store = inject(StoreService);

  isLoading = false;
  err: string | null = null;

  selectedDir: string | null = null;

  dockerComposeFileInfo: ReadFileInfo | null = null;

  async ngOnInit() {
    if (!this.store.getSelectedDirectory()) {
      this.router.navigate(['/']);
    }

    this.selectedDir = this.store.getSelectedDirectory();

    await this.findDockerComposeFile();
  }

  async findDockerComposeFile() {
    this.isLoading = true;
    this.err = null;
    this.dockerComposeFileInfo = null;

    if (!checkApi()) {
      this.err = 'Electron API not working';
      this.isLoading = false;
      return;
    }

    var api = getApi();

    var files = await api.readFiles(this.store.getSelectedDirectory()!, {
      fileExts: new Set(),
      fileNames: new Set(['docker-compose.yml']),
      ignoreDirs: new Set(),
      ignoreFileNames: new Set(),
    });

    if (files.length < 1) {
      this.err = 'Failed to find docker compose file in directory';
      this.isLoading = false;
      return;
    }

    if (files.length > 1) {
      this.err = 'Found two docker compose files';
      this.isLoading = false;
      return;
    }

    this.dockerComposeFileInfo = files[0];
    this.isLoading = false;
  }
}
