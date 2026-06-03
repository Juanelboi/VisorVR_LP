# Viror VR

Proyecto de realidad virtual desarrollado en Unity con XR Interaction Toolkit.

## Requisitos

- **Unity Hub** instalado ([descargar aquí](https://unity.com/download))
- **Unity 6** (la versión exacta se encuentra en `ProjectSettings/ProjectVersion.txt`)
- Módulo **Android Build Support** + **OpenJDK** + **Android SDK & NDK Tools** (para builds en Quest)
- **Meta Quest Developer Hub** o ADB para instalar en el visor (opcional)

## Cómo clonar y abrir el proyecto

### 1. Clonar el repositorio

```bash
git clone <URL-del-repositorio>
cd "Viror VR"
```

### 2. Abrir en Unity Hub

1. Abre **Unity Hub**.
2. Haz clic en **Add** (o **Añadir proyecto**) → **Add project from disk**.
3. Selecciona la carpeta raíz del repositorio (`Viror VR/`).
4. Unity Hub detectará la versión requerida en `ProjectSettings/ProjectVersion.txt`.  
   Si no la tienes instalada, Unity Hub te ofrecerá descargarla.
5. Haz clic en el proyecto para abrirlo.

> **Primera apertura:** Unity regenerará automáticamente la carpeta `Library/` (esto puede tardar varios minutos).

### 3. Verificar los paquetes

Una vez abierto el editor, Unity importará los paquetes definidos en `Packages/manifest.json`.  
Si aparece algún error de paquete faltante, ve a **Window → Package Manager** y verifica que estén instalados:

- XR Interaction Toolkit
- XR Plugin Management
- Cesium for Unity (si aplica)
- TextMeshPro

### 4. Configurar XR

1. Ve a **Edit → Project Settings → XR Plug-in Management**.
2. En la pestaña **Android**, activa **OpenXR** (para Meta Quest).
3. Añade el perfil de interacción correspondiente en **OpenXR → Interaction Profiles**.

## Estructura del repositorio

```
Assets/           → Escenas, scripts, assets del proyecto
Packages/         → Dependencias de Unity Package Manager
ProjectSettings/  → Configuración del proyecto (build, physics, input, XR...)
```

## Build para Meta Quest

1. Ve a **File → Build Settings**.
2. Selecciona plataforma **Android** y haz clic en **Switch Platform**.
3. Conecta el visor en modo desarrollador (o usa **Meta Quest Link**).
4. Haz clic en **Build And Run** para compilar e instalar directamente.

## Notas

- La carpeta `Library/` está excluida del repositorio por `.gitignore`; Unity la regenera al abrir el proyecto.
- Los archivos `.csproj` y `.sln` también se regeneran automáticamente por Unity.
- `UserSettings/` contiene preferencias locales del editor y no se versiona.
