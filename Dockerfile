FROM mcr.microsoft.com/dotnet/sdk:11.0-resolute AS build
WORKDIR /src
ARG TARGETARCH

COPY FileTools/FileTools.csproj FileTools/
COPY FileToolsCli/FileToolsCli.csproj FileToolsCli/
RUN dotnet restore FileToolsCli/FileToolsCli.csproj -a "$TARGETARCH" -p:UseAppHost=false

COPY FileTools/ FileTools/
COPY FileToolsCli/ FileToolsCli/
RUN dotnet publish FileToolsCli/FileToolsCli.csproj -c Release -a "$TARGETARCH" \
    --no-restore -p:UseAppHost=false -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:11.0-resolute
WORKDIR /app
RUN mkdir -p /var/lib/filetools && chown app /var/lib/filetools
COPY --from=build /app/publish/ ./
USER app
ENTRYPOINT ["dotnet", "FileToolsCli.dll"]
