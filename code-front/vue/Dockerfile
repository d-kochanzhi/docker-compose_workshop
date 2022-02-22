# этап сборки (build stage)
FROM node:lts-alpine as build-stage
# делаем каталог 'app' текущим рабочим каталогом
WORKDIR /app
# копируем оба 'package.json' и 'package-lock.json' (если есть)
COPY package*.json ./
# устанавливаем зависимости проекта
RUN npm install
# копируем файлы и каталоги проекта в текущий рабочий каталог (т.е. в каталог 'app')
COPY . .

#Передаем переменные  ARG из docker-compose -> dockerfile -> webpack.config.js
# Build args
ARG VUE_APP_ROOT_API
# Environment vars
ENV VUE_APP_ROOT_API=$VUE_APP_ROOT_API


# собираем приложение для production с минификацией
RUN npm run build

# этап production (production-stage)
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/index.html /usr/share/nginx/html
COPY --from=build-stage /app/dist /usr/share/nginx/html/dist
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
