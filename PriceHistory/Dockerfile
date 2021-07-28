FROM mcr.microsoft.com/playwright
RUN apt-get update; \
    apt-get install wget apt-transport-https --no-install-recommends && \
    wget https://download.visualstudio.microsoft.com/download/pr/841cf439-e6c6-4164-ae75-e4f1f3aafe0c/fd64b0395f6f0dee9ed184d918e49cd7/dotnet-runtime-6.0.0-preview.6.21352.12-linux-x64.tar.gz -O dotnet.tar.gz && \
    tar vzxf dotnet.tar.gz -C /usr/local/bin && \
    rm dotnet.tar.gz && \
    apt-get clean && apt-get autoclean
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "PriceHistory.dll"]