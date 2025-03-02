[ENGLISH README](./README.md)

# Reality Fusion
Привет! Это MR приложение для любого смартфона Android 9+. Оно имеет отслеживание рук, отслеживание 3DoF, браузер и другие классные вещи. Оно использует [mediapipe unity plugin 0.16](https://github.com/homuler/MediaPipeUnityPlugin) для отслеживания рук и [google cardboard 1.28](https://github.com/googlevr/cardboard-xr-plugin) для VR и стереозрения.

## Если вы просто хотите его использовать...
Выберите меню «Releases» в правой части экрана и загрузите APK-файл из последнего релиза. Затем установите его на свой телефон и запустите.

## Если вы хотите скомпилировать этот проект...

!!ИСПОЛЬЗУЙТЕ UNITY 2022.3.35f1, УСТАНОВЛЕННЫЙ С ANDROID BUILDING TOOLS!!
Импортируйте проект в Unity, открыв его из Unity Hub, затем подключите свой телефон с помощью кабеля USB и включите режим отладки USB. Разрешите установку приложений из ADB.

Установите конфигурацию Unity следующим образом (по сути, она устанавливается автоматически):

### Настройки проекта
### Плеер
#### Разрешение и представление
![image](https://github.com/ZernovTechno/AR/assets/90546939/a37b0eda-85c2-4c09-a83c-4e5bcf3da646)

#### Другие настройки
![image](https://github.com/ZernovTechno/AR/assets/90546939/6ccac38f-c521-406d-8782-dbe65974547b)

#### Публикация Настройки
![image](https://github.com/ZernovTechno/AR/assets/90546939/07f3d81a-a2b9-4af5-9bde-126a721199a9)

А затем откройте сцену в проводнике в нижней части Unity. Сцена находится в Assets->MediaPipeUnity->Samples->Scenes->Hand Landmark Detection->Hand Tracking.unity

Здесь вы можете делать то, что хотите.

Затем нажмите File->Build Settings->Add Open Scenes, отметьте добавленную сцену.

Теперь вы можете собрать и запустить на телефоне. Проверьте подключение телефона и нажмите «Build And Run» в меню Build Settings/File.

## Поздравляю!
