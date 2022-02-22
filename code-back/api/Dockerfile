# Основные
# FROM - родительский образ
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# WORKDIR - установка рабочей директории для инструкций RUN, CMD, ENTRYPOINT, COPY и ADD
WORKDIR /app
# Инструкция EXPOSE определяет порты, на которых контейнер регистрирует соединения.
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
# COPY - копирование новых файлов или директорий из <src>
# и их добавление в файловую систему образа по адресу, указанному в <dest>
COPY ["dockerapi.csproj", ""]
# RUN - выполнение команды в новом слое на основе текущего образа и фиксация результата
# RUN выполняет команду и фиксирует результат,
# CMD ничего не выполняет во время сборки, а определяет команду для образа
# (!) выполняется только одна (последняя) инструкция CMD
RUN dotnet restore "./dockerapi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "dockerapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dockerapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT - настройка исполняемого контейнера
ENTRYPOINT ["dotnet", "dockerapi.dll"]
# разница между ENTRYPOINT и CMD:
# https://docs.docker.com/engine/reference/builder/#understand-how-cmd-and-entrypoint-interact
# https://stackoverflow.com/questions/21553353/what-is-the-difference-between-cmd-and-entrypoint-in-a-dockerfile