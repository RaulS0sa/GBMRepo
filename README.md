# GBMRepo
 This Is A Library To comunicate with GBM Broker through C# 
 
 Include The GBM namespace in your project
 Link To libraries
 
 https://www.nuget.org/packages/GBMProyect/
 
 https://pypi.org/project/GBMProyect/0.0.1/
 
 
 ![alt text](https://i.ibb.co/r4LJ0fr/ClientID.png)
 
 ![alt text](https://i.ibb.co/f9N5F8j/Contract-ID.png)
 
 How Do You use it (Python sample):
 
 ```
from GBMProyect import GBM
import time
TraderObj = GBM("Mymail@mail.com","my_password", "clientID", "contractID")

#The library can trwo an error from request dependency if your session is alredy open
#in your mobile app or in browser
#All the operations return None if the login timesout or an error happens
# Always check for None and relog

AmILogged = False
while True:
    BMVMovers = TraderObj.GetNationalMarketMovers()
    if BMVMovers is not None:

        # GetTickerHistorical can return None if the Ticker names is misspeled
        # or you are not logged in
        TickerData = TraderObj.GetTickerHistorical("AMX L", 20)

        # GetAllPositions can return None if you have no Positions
        # or you are not logged in
        MyPositions = TraderObj.GetAllPositions()

        if TickerData.CurrentPrice <= 14.15:
            FinancialData = TraderObj.GetContractBuyingPower()
            QuantityToBuy =int(FinancialData.buyingPower / TickerData.CurrentPrice)
            BuyOrder = TraderObj.GenerateOrder("AMX L",
                                               QuantityToBuy,
                                               GBM.OrderTypes.Buy,
                                               TickerData.CurrentPrice,
                                               GBM.InstrumentTypes.IPC)
        elif TickerData.CurrentPrice > 14.50:
            PositionByTicker = TraderObj.GetPositionByTicker("AMX L")
            if PositionByTicker is not None:
                SellOrder = TraderObj.GenerateOrder("AMX L",
                                                    PositionByTicker.quantity,
                                                    GBM.OrderTypes.Sell,
                                                    TickerData.CurrentPrice,
                                                    GBM.InstrumentTypes.IPC)

    else:
        AmILogged = TraderObj.Autenticate()
    time.sleep(1)
 ```
 
