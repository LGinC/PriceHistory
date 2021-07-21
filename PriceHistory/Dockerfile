FROM mcr.microsoft.com/playwright
RUN apt-get update; \
    apt-get install wget apt-transport-https && \
    wget https://packages.microsoft.com/config/ubuntu/20.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && apt-get install -y dotnet-runtime-5.0 && \
    apt-get clean
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "PriceHistory.dll"]