import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WebGLService {

  private getWebGLContext(): WebGLRenderingContext | null {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('webgl') || canvas.getContext('experimental-webgl');
    return context as WebGLRenderingContext | null;
  }

  getWebGLInfo(): any {
    const context = this.getWebGLContext();

    if (!context) {
      return { error: 'WebGL not supported by this browser or device.' };
    }

    // Retrieve WebGL information
    const gl = context;
    const info = {
      renderer: gl.getParameter(gl.RENDERER),
      vendor: gl.getParameter(gl.VENDOR),
      version: gl.getParameter(gl.VERSION),
      shadingLanguageVersion: gl.getParameter(gl.SHADING_LANGUAGE_VERSION),
      maxTextureSize: gl.getParameter(gl.MAX_TEXTURE_SIZE),
      maxCubeMapTextureSize: gl.getParameter(gl.MAX_CUBE_MAP_TEXTURE_SIZE),
      maxRenderbufferSize: gl.getParameter(gl.MAX_RENDERBUFFER_SIZE),
      maxVertexAttributes: gl.getParameter(gl.MAX_VERTEX_ATTRIBS),
      maxVertexUniformVectors: gl.getParameter(gl.MAX_VERTEX_UNIFORM_VECTORS),
      maxFragmentUniformVectors: gl.getParameter(gl.MAX_FRAGMENT_UNIFORM_VECTORS),
      maxVaryingVectors: gl.getParameter(gl.MAX_VARYING_VECTORS),
      maxTextureImageUnits: gl.getParameter(gl.MAX_TEXTURE_IMAGE_UNITS),
      maxCombinedTextureImageUnits: gl.getParameter(gl.MAX_COMBINED_TEXTURE_IMAGE_UNITS),
      maxVertexTextureImageUnits: gl.getParameter(gl.MAX_VERTEX_TEXTURE_IMAGE_UNITS),
      maxFragmentTextureImageUnits: gl.getParameter(gl.MAX_TEXTURE_IMAGE_UNITS),
      textureCompression: gl.getParameter(gl.COMPRESSED_TEXTURE_FORMATS),
      supportedExtensions: gl.getSupportedExtensions(),
    };

    return info;
  }

  // Function to get WebGL extension information
  getWebGLExtensionInfo(): any {
    const context = this.getWebGLContext();

    if (!context) {
      return { error: 'WebGL not supported by this browser or device.' };
    }

    const gl = context;
    const extensions: any = {};

    // Get all supported WebGL extensions
    const supportedExtensions = gl.getSupportedExtensions();
    if (supportedExtensions) {
      supportedExtensions.forEach((ext: string) => {
      const extension = gl.getExtension(ext);
      });
    }

    return extensions;
  }

  // Function to get information about the WebGL version
  getWebGLVersion(): string {
    const context = this.getWebGLContext();

    if (!context) {
      return 'WebGL not supported';
    }

    const gl = context;
    const version = gl.getParameter(gl.VERSION);
    return version;
  }
}
