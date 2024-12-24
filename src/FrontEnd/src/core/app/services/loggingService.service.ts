// logging.service.ts
import { Injectable } from '@angular/core';
import DevelopmentConfig from '../../../environments/development';

export enum LogLevel {
  Debug,
  Info,
  Warn,
  Error
}

@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  private level = DevelopmentConfig.LogLevel

  debug(message: string, ...args: any[]): void {
    this.log(LogLevel.Debug, message, args);
  }

  info(message: string, ...args: any[]): void {
    this.log(LogLevel.Info, message, args);
  }

  warn(message: string, ...args: any[]): void {
    this.log(LogLevel.Warn, message, args);
  }

  error(message: string, ...args: any[]): void {
    this.log(LogLevel.Error, message, args);
  }

  private log(level: LogLevel, message: string, args: any[]): void {
    if (level >= this.level) {
      const timestamp = new Date().toISOString();
      const logMessage = `[${timestamp}] ${LogLevel[level]}: ${message}`;
      
      switch (level) {
        case LogLevel.Debug:
          console.debug(logMessage, ...args);
          break;
        case LogLevel.Info:
          console.info(logMessage, ...args);
          break;
        case LogLevel.Warn:
          console.warn(logMessage, ...args);
          break;
        case LogLevel.Error:
          console.error(logMessage, ...args);
          break;
      }
    }
  }
}