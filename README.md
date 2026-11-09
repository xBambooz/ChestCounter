> Cambios del fork: fork de Ryan de [xdoofy92/ChestCounter](https://github.com/xdoofy92/ChestCounter), con seguimiento de compañeros y archivo opcional de replays.

<div align="center">

**Español** · [English](README.en.md)

# Contador de cofres de Fortnite

[![version](https://img.shields.io/github/v/release/xdoofy92/ChestCounter?style=flat-square&label=versión&color=e8b04b)](../../releases/latest) [![descargas](https://img.shields.io/github/downloads/xdoofy92/ChestCounter/total?style=flat-square&label=descargas&color=e8b04b)](../../releases) ![Windows 10 | 11](https://img.shields.io/badge/Windows-10%20%7C%2011-3a7bd5?style=flat-square) [![licencia](https://img.shields.io/github/license/xdoofy92/ChestCounter?style=flat-square&label=licencia&color=555)](LICENSE)

**Cuenta cada cofre que abres en Fortnite, partida tras partida, con un overlay sobre el juego.**

<img src="docs/overlay-espera.png" alt="Overlay entre dos partidas" width="300">&nbsp;&nbsp;<img src="docs/overlay-en-curso.png" alt="Overlay durante una partida" width="300">

[**⬇ Descargar ChestCounter.exe**](../../releases/latest) · Windows 10 / 11 · gratis · sin instalación

</div>

---

Contador de cofres lee las repeticiones (replays) que Fortnite guarda después de cada partida y lleva la cuenta de los cofres que has abierto, además de los **tesoros de la caza del tesoro** que te han dado. Un overlay siempre visible muestra tus totales mientras juegas, y un informe detallado guarda todo tu historial.

> Cuenta los cofres que **tú** abriste. No muestra dónde están los cofres en el mapa.

## Cómo funciona (y por qué es seguro)

Fortnite puede grabar un **archivo de repetición** de cada partida en tu PC. Contador de cofres lee estos archivos cuando la partida ha terminado, y nada más.

- ✅ **Solo lee archivos de repetición**: los archivos `.replay` que Fortnite escribe en `%LOCALAPPDATA%\FortniteGame\Saved\Demos`. Se abren en modo solo lectura y nunca se modifican.
- ✅ **Sin modificar el juego**: no se toca nada de la carpeta de instalación de Fortnite, no se añade ni se parchea ningún archivo.
- ✅ **Sin acceso a la memoria del juego**: la herramienta nunca lee, escribe ni inyecta nada en el proceso de Fortnite. No interactúa en absoluto con el juego en marcha.
- ✅ **No envía datos a ningún sitio**: sin inicio de sesión, sin acceso a tu cuenta de Epic, sin telemetría. El único acceso a la red es la comprobación de nuevas versiones en GitHub, que se puede desactivar en los ajustes.
- ✅ **Ligero**: funciona con prioridad baja (y en "modo de eficiencia" en Windows 11), duerme entre partidas, y el overlay solo se vuelve a dibujar cuando cambia un número.

> [!IMPORTANT]
> **Las repeticiones deben estar activadas en Fortnite**: *Ajustes → Juego → Repeticiones → Grabar repeticiones: **Activado*** (y *Modos de equipos grandes* si los juegas). Sin repeticiones, la herramienta no tiene nada que leer.

Como Fortnite solo guarda la clave de descifrado de la repetición cuando termina la partida, los cofres se cuentan **unos segundos después de cada partida**, no durante ella. Mientras juegas, el overlay muestra **● EN PARTIDA**.

## Primeros pasos

1. Descarga **[`ChestCounter.exe`](../../releases/latest)**. Ese único archivo es todo lo que necesitas.
2. Activa las repeticiones en Fortnite (ver arriba).
3. Ejecuta `ChestCounter.exe`.
   > Windows puede mostrar *"Windows protegió tu PC"* porque el exe no está firmado. Haz clic en *Más información → Ejecutar de todas formas*.
4. Una breve configuración inicial (cuadros de diálogo Sí / No) comprueba tu **carpeta de repeticiones**, ofrece contar las **repeticiones que ya tienes en el PC** y ofrece **abrir el contador automáticamente cuando se inicie Fortnite**. El idioma sigue al de Windows: **español** si Windows está en español, **inglés** en cualquier otro caso.

<p align="center"><img src="docs/setup.png" alt="Configuración del primer inicio" width="760"></p>

Si cuentas tus repeticiones existentes, una barra de progreso avanza en la ventana mientras el informe se abre en tu navegador y se rellena en directo.


5. Deja la ventana abierta (puedes minimizarla) mientras juegas. Unos segundos después de cada partida, sus cofres se suman:

<p align="center"><img src="docs/main.png" alt="Ventana principal" width="820"></p>

> [!TIP]
> Fortnite solo guarda un número limitado de repeticiones no guardadas y borra las más antiguas, y una partida jugada mientras el contador está cerrado no se cuenta. Activa **Abrir con Fortnite** (se ofrece en la configuración inicial, o en los ajustes de la ventana) para no tener que pensar en ello.

## Abrir con Fortnite

Con esta opción activada, un pequeño proceso invisible se inicia con Windows y comprueba cada pocos segundos si Fortnite está abierto. No lee nada del juego: solo mira la lista de programas en ejecución. Cuando Fortnite se inicia, abre el contador (minimizado, para que el juego conserve la pantalla). Cuando Fortnite se cierra, el contador cuenta tu última partida y se cierra 30 segundos después; haz clic en el aviso de su ventana para mantenerlo abierto. Puedes desactivarlo en cualquier momento en los ajustes (botón **Ajustes**): el proceso invisible se detiene y se quita del inicio de Windows.

## El overlay

- **Cifras grandes**: tu **total** de cofres (dorado) y, en el recuadro, los cofres de la partida **anterior** (o de la **actual** mientras juegas).
- **Sesión**: cofres desde que se abrió la herramienta. **Media**: media de cofres por partida en esta sesión.
- **Anterior** (solo durante una partida): cofres de tu partida anterior. **Tesoros**: tesoros encontrados hasta ahora.
- Durante una partida aparece la etiqueta **EN PARTIDA** y el recuadro muestra "…" hasta que se pueden leer los cofres; entonces pasa a **EN DIRECTO** con los cofres de la partida actual.

Para verlo sobre el juego, pon Fortnite en **Pantalla completa en ventana** (*Ajustes → Vídeo → Modo de ventana*): en pantalla completa exclusiva, Windows no puede dibujar nada encima del juego. El overlay viene **desactivado**: actívalo con el botón **Overlay** (punto verde = activado, rojo = desactivado) o con Ctrl+Mayús+O.

¿Transmites en directo? Mientras el contador está abierto, el mismo overlay está en **http://localhost:7373/** (cópialo desde **Ajustes**). En OBS, añade una **fuente de navegador** con esa dirección: tiene fondo transparente y se actualiza sola.

## Atajos

| Atajo | Qué hace |
|---|---|
| **Ctrl+Mayús+O** | Mostrar / ocultar el overlay. Funciona en cualquier sitio, incluso en el juego. |
| **Ctrl+Mayús+L** | Bloquear / desbloquear el overlay. Funciona en cualquier sitio. **Bloqueado**: el ratón lo atraviesa y no se puede mover (ideal en el juego). **Desbloqueado**: mantén el botón izquierdo para moverlo. |
| Clic derecho en el overlay | Ocultarlo. |
| Candado en el overlay | Bloquearlo. |

Si otro programa ya usa Ctrl+Mayús+O o Ctrl+Mayús+L, la herramienta lo indica y el overlay sigue funcionando con su botón en la ventana.

### Ventana principal

La ventana tiene tres botones: **Overlay** (mostrar u ocultar el overlay; punto verde = activado, rojo = desactivado), **Informe** (abre `report.html`) y **Ajustes**. La versión instalada aparece en el título de la ventana. Si hay una versión nueva, aparece un botón para instalarla; cuando Fortnite se cierra, un aviso permite mantener el contador abierto (haz clic en él). El selector de perfil aparece cuando hay varias cuentas.

La ventana se puede **redimensionar y maximizar**: todo crece en proporción (tarjetas, cifras y gráfico) y ningún texto se corta; si una cifra no cabe en su tarjeta, su letra se reduce un poco.

En **Ajustes** puedes abrir con Fortnite, abrir el informe al iniciar, elegir las actualizaciones, escribir los **cofres abiertos antes del contador** (un estimado que se suma a tu total), copiar la **dirección del overlay para OBS** y cambiar la carpeta de repeticiones. Abajo se ve la versión instalada.

<p align="center"><img src="docs/settings.png" alt="Ajustes" width="400"></p>

De arriba abajo verás: el total de cofres, la última partida, la sesión, hoy, el día con más cofres, la media, la mejor partida, el **Histórico** (partidas guardadas y desde cuándo), los tesoros y el gráfico de los últimos 7 días.

## El informe

`report.html` se abre en tu navegador y se actualiza solo. Usa los mismos colores que la ventana (oscuro, o claro si Windows está en modo claro) y se adapta al móvil:

- **Cifras clave:** el total de cofres en grande, con los de la sesión y tus victorias reales; al lado, partidas, media por partida, récord y tesoros.
- **Cofres por día:** gráfico de los últimos 30 días con escala, la media diaria (línea discontinua) y hoy en cian. Pasa el ratón por una barra para ver el día y sus cofres.
- **Por tipo de cofre** y **de dónde vienen los tesoros**, con una barra de proporción por fila. Las cajas de munición, de comida y los cañones de botín aparecen aparte, fuera del total.
- **Tesoros obtenidos:** cada tesoro con su partida, el momento y el cofre de origen.
- **Últimas partidas** (las 100 más recientes): modo, puesto (🏆 victoria en dorado, top 10 en verde), cofres y tesoros.

<p align="center"><img src="docs/report.png" alt="Informe" width="820"></p>

## Tesoros

- **★★ Cofre dorado (seguro).** Cuando un cofre da un premio de la caza del tesoro (cosmético, pista musical, pantalla de carga, vehículo…), el servidor genera un cofre dorado que lleva el **id del premio**. La herramienta lo lee de la repetición.
- **★ Cada tesoro recibido (momento exacto).** Cada tesoro que recibes se guarda con su hora exacta y el cofre del que salió; solo cuenta si ese cofre es **tuyo** (los tesoros compartidos con un compañero no se cuentan). Cuando se puede identificar el objeto, se le da nombre; si no, aparece como **"Artilugio de tesoro"** (fuego artificial dorado, extractor portátil, localizadores, acelerador de extracción), cuyo tipo exacto no está en la repetición.
- Un tesoro que sale de algo que no es un cofre (cañón de botín, caja de munición…) se resalta con ⚑ en el informe.

El juego solo envía ids de premios, no nombres: `treasures.json` los asigna a nombres y se puede editar con el Bloc de notas. Los ids nuevos se añaden solos y la herramienta te avisa cuando uno necesita nombre.

## Perfiles (varias cuentas en un PC)

Las repeticiones guardadas en un PC suelen ser las de su jugador, así que **no hay nombre que escribir**: la herramienta cuenta al jugador que grabó cada repetición. Si se encuentran repeticiones de varias cuentas, cada cuenta tiene su propio perfil con sus estadísticas. Arriba a la derecha de la ventana, el texto **Perfil** se convierte entonces en un **desplegable** para elegir la cuenta que se muestra (en la ventana, el overlay, OBS y el informe). Una partida jugada en otra cuenta se guarda en el perfil de esa cuenta, nunca se pierde.

## Actualizaciones

Al iniciarse, el contador comprueba en GitHub si hay una nueva versión. Si la hay, te avisa, y al pulsar el botón de instalación se descarga, se verifica y se reinicia el contador: tu historial y tus ajustes se conservan. En los ajustes (botón **Ajustes**) puedes elegir entre *ofrecer* (por defecto), *instalar automáticamente* (solo entre dos partidas) o *desactivadas* (ningún acceso a la red).

## Tus datos

Todo se guarda en `%APPDATA%\ChestCounter`:

| Archivo | Contenido |
|---|---|
| `report.html` | El informe (se abre en tu navegador). |
| `overlay.html` | Copia del overlay en un archivo (para OBS es mejor la dirección http://localhost:7373/). |
| `matches.csv` | Una línea por partida, para Excel o Google Sheets. |
| `data.json` | Tu historial completo. **Haz copia de seguridad** si cambias de PC. |
| `config.json` | Perfil, carpeta de repeticiones, posición del overlay y ajustes. |
| `treasures.json` | Nombres de los premios de la caza del tesoro. |
| `errors.log` | Detalles de las repeticiones que no se pudieron leer, para adjuntar a un informe de error. |

Fortnite borra los replays antiguos, pero **tu historial no se borra**: cada partida se guarda en `data.json` en cuanto se cuenta, y el informe y las estadísticas salen de ahí. Por eso, aunque el replay ya no exista, la partida sigue en tus totales.

Para empezar de cero: ejecuta `ChestCounter.exe --reset-config`, o borra esa carpeta.

## Preguntas frecuentes

**No se cuenta nada.** Comprueba que las repeticiones están activadas en Fortnite y que la herramienta está en marcha cuando termina la partida. La ventana te indica si falta la carpeta de repeticiones o está vacía.

**¿Se cuentan las cajas de munición?** No. Las cajas de munición, las cajas de comida y los cañones de botín aparecen aparte en el informe y no forman parte del total de cofres. Los cofres que **destruyes** en vez de abrirlos sí se cuentan.

**¿Modos creativos?** Solo se cuentan las partidas de Battle Royale; los demás modos se omiten.

**"No se puede leer esta repetición".** Una actualización de Fortnite puede cambiar el formato de las repeticiones, y entonces el lector necesita una actualización. Las partidas ya contadas se conservan. Abre un [issue](../../issues) y adjunta `errors.log` (en `%APPDATA%\ChestCounter`).

**¿Ya está abierto?** Solo puede haber un contador en marcha: si lo abres otra vez, te avisa de que ya está abierto.

**¿Una partida puede contarse dos veces?** No, aunque renombres o guardes la repetición.

## Opciones de línea de comandos

```
ChestCounter.exe --demos <carpeta>   otra carpeta de repeticiones
ChestCounter.exe --data <carpeta>    otra carpeta de datos
ChestCounter.exe --once              analiza las repeticiones terminadas y se cierra
ChestCounter.exe --reset-config      vuelve a hacer la configuración inicial
ChestCounter.exe --overlay-preview <archivo.png>   guarda vistas previas del overlay y se cierra
```

## Compilar desde el código

Instala el [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0) (Windows x64) y haz doble clic en **`build.bat`**: el exe se genera en `dist\ChestCounter.exe`. El flujo de GitHub Actions compila el exe en cada push y lo adjunta a una versión cuando se sube una etiqueta `v*`.

## Licencia y créditos

[MIT](LICENSE). El análisis de las repeticiones se basa en [FortniteReplayDecompressor](https://github.com/Shiqan/FortniteReplayDecompressor) de Shiqan (MIT), ligeramente modificado: ver [`lib/PATCHES.md`](lib/PATCHES.md).

Este proyecto no está afiliado a Epic Games, ni cuenta con su respaldo ni patrocinio. Fortnite es una marca registrada de Epic Games, Inc.

## Equipo y archivo de replays

El informe muestra totales de equipo, compañeros por partida y totales agrupados por nombre. Haz clic en Histúrico en la ventana principal para ver el desglose. El total de equipo incluye tus cofres y los observados de compañeros sin cambiar tu cuenta personal. El replay solo registra compañeros cercanos o espectados, por lo que puede faltar información. No se deducen tesoros de compañeros. Las partidas antiguas se vuelven a analizar una vez si aún existe el replay en Demos o en el archivo.

Ajustes permite activar Equipo en overlay y Archivar replays (desactivado por defecto), elegir carpeta y límite en GB. La carpeta predeterminada es `%LOCALAPPDATA%\ChestCounter\ReplayArchive`. Se copian replays finalizados y analizados correctamente; al superar el límite se eliminan primero los archivados más antiguos. Demos nunca se modifica. El tamaño se muestra al abrir Ajustes. El análisis incluye el archivo sin duplicar nombres de archivo ni partidas. Las estadísticas usan todo el historial. No se pueden recuperar replays ya eliminados y futuras versiones del juego pueden necesitar actualizaciones del analizador.

No se modifica ningún ini de Fortnite: no se verific? un ajuste compatible para el límite automático. Epic distingue los replays automáticos de los guardados, ilimitados en PC. Consulta [el soporte de Epic](https://www.epicgames.com/help/c-34254770/c-33726977/fortnite-a25553277).

Escuadra agrupa compañeros observados por cuenta (o nombre), junto a tu fila resaltada. Muestra partidas, cofres y media, ordenados por `(total + mediaDeCompañeros * 5) / (partidas + 5)`. Quienes tienen menos de tres partidas observadas aparecen atenuados al final. Está en el informe y en la pestaña Escuadra tras pulsar Histúrico.
