import requests
from enum import Enum
from datetime import datetime, timezone
import json
import calendar
import pytz
import urllib

class GBM():
    def __init__(self, userMail, password, clientID,contractID):
        self.User = userMail
        self.Password = password
        self.ClientID = clientID
        self.ContractID = contractID
        self.BearerKey = "eyJraWQiOiJSUXRsSzlNbUtJT1JTNjA4alVpazBPZktNeHlEXC81UEUzenp6SGNqNVlJYz0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJjODIxYmI4MS0xZGMwLTQzYzYtOWI0ZC1mYzYwNDllOTJkNzgiLCJldmVudF9pZCI6IjQ2ODVhZjU4LWFkY2QtNDQ1MS04MDExLTU2MGVlYjZkOTRjNiIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE2MDk1NDIyMDUsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX0JLdTdxQW9odSIsImV4cCI6MTYwOTU0NTgwNSwiaWF0IjoxNjA5NTQyMjA1LCJqdGkiOiIyNjIxYWE1Mi0xOGUxLTQzYjEtOTU1Ny1lNTk2NDFkZDY3MmUiLCJjbGllbnRfaWQiOiI3cmszZ3FiaDRsb2NkY2dibXZkanEwaGRlNSIsInVzZXJuYW1lIjoicmF1bC5zb3NhLmNvcnRlc3xnbWFpbC5jb20ifQ.cA1gd29iAJoy10CxQcIhQSnhu3Kv-AU_ZqPgXQZrEgrm23a0czHk3-x5fCQhM85Zh8oseb8cmcUnb0WQCNgzt5Wa5I0eUsboLlMh5ULY8APG4y3eRUELjA0uAtcHiGLyyfdpnQddykBO86wheYb5J3_W33CBtZkDqNarmE_UwVL6JLxYbJsXU4S6mbw_VG2-akSTQLrOEo1HwffzGnvrGTuVYMFDyFSz1jTxhOZR7wkdm6AMhC85O8qAfjN701fFEckTBuMBoii_hKFbDTpBWI8OrX-XiraHRRzk4UQwPt2E_AyTSFqWFeDObyJ2gDztO1maCErl0w9Yztpb5cV7jw";
    def Autenticate(self) -> bool:
        WebServiceURL = "https://auth.gbm.com/api/v1/session/user"
        DataStream = {"clientid": self.ClientID, "user": self.User, "password": self.Password}
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            self.BearerKey = Response["accessToken"]
            return True
        else:
            return False
    def GetWithDrawalAmount(self, ContractID):
        if ContractID is not None:
            WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/" \
                            + "Operation" \
                            + "/GetWithdrawalAmount"
            DataStream = {"contractId": ContractID}
            Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
            if Response:
                return Response
            else:
                return None
        else:
            return None
    def FetchStateOfTransfer(self,SmartCashID, TransferID):
        MainContractRequest = self.__GetMainContract()
        if MainContractRequest is not None:
            WebServiceURL = "https://api.gbm.com/v1/contracts/" \
                            + MainContractRequest \
                            + "/accounts/" \
                            + SmartCashID \
                            + "/cash-transactions"
            Response = self.__Generate_GET_Request(WebServiceURL, None)
            if Response:
                ArrayOfTransactions = Response["items"]
                for Transaction in ArrayOfTransactions:
                    if Transaction["transfer_id"] == TransferID:
                        return Transaction
                return None
        else:
            return None
    def Transfer(self, Amount, Origin, Destiny):
        MainContractRequest = self.__GetMainContract()
        if MainContractRequest is not None:
            WebServiceURL = "https://api.gbm.com/v1/contracts/" \
                            + MainContractRequest \
                            + "/accounts/" \
                            + Origin \
                            + "/transfers"
            DataStream = {"amount": Amount, "target_account_id": Destiny}
            Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
            if Response:
                return Response
            else:
                return None
        else:
            return None
    def GetContracts(self)-> dict:
        MainContractRequest = self.__GetMainContract()
        if MainContractRequest is not None:
            WebServiceURL = "https://api.gbm.com/v2/contracts/" + MainContractRequest + "/accounts"
            Response = self.__Generate_GET_Request(WebServiceURL, None)
            if Response:
                response_dict = {element["name"]: element for element in Response}
                return response_dict
            else:
                return None
        else:
            return None
    def __GetMainContract(self) -> str:
        WebServiceURL = "https://api.gbm.com/v1/contracts"
        Response = self.__Generate_GET_Request(WebServiceURL, None)
        if Response:
            return Response[0]["contract_id"]
        else:
            return None
    def GetNationalMarketMovers(self):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Market/GetLowRiseIssuesByBenchmark"
        DataStream = {"request": 1}
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            MoversResponse = self.MarketMovers(Response[0:10], Response[10:20], Response[19:29])
            return MoversResponse
        else:
            return None
    def GetGlobalMarketMovers(self):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Market/GetLowRiseIssuesByMarket"
        DataStream = {"instrumentType": 2, "isOnLine": True}
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            MoversResponse = self.MarketMovers(Response[0:10], Response[10:20], Response[19:29])
            return MoversResponse
        else:
            return None
    def GetContractBuyingPower(self):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Operation/GetContractBuyingPower";
        DataStream = {"request": self.ContractID}
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            ContactPower = self.contractBuyingPower(Response)
            return ContactPower
        else:
            return None

    def CancelOrder(self, OrderID,vigencia ):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Operation/CancelOrder"
        DataStream = {"electronicOrderId": OrderID, "vigencia": vigencia, "isPreDispatchOrder": False }
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            #ContactPower = self.contractBuyingPower(Response)
            return Response["response"]
        else:
            return None
    def GenerateOrder(self, Ticker,quantity,orderType,Price,instrumentType):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Operation/RegisterCapitalOrder"
        DataObject = self.PurchaseOrderObject(self.ContractID, Ticker, quantity, orderType, Price, instrumentType)
        DataStream =vars(DataObject)
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            OrderResponse = self.OrderResponse(Response)
            return OrderResponse
        else:
            return None

    def __GetPositionSummary(self):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Portfolio/GetPositionSummary"

        DataStream = {"request": self.ContractID}
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            return Response
        else:
            return None
    def GetAllPositions(self):
        PositionData = self.__GetPositionSummary()
        if(PositionData is not None):
            ListOfPositions = list()
            if PositionData.get("mercadosGlobalesSIC"):
                for i in PositionData["mercadosGlobalesSIC"]:
                    ListOfPositions.append(self.Tickerdata(i))
            if PositionData.get("mercadoCapitales"):
                for i in PositionData["mercadoCapitales"]:
                    ListOfPositions.append(self.Tickerdata(i))
            return ListOfPositions
        else:
            return None
    def GetPositionByTicker(self, Ticker):
        PositionData = self.__GetPositionSummary()
        if(PositionData is not None):
            ListOfPositions = dict()
            if PositionData.get("mercadosGlobalesSIC"):
                for i in PositionData["mercadosGlobalesSIC"]:
                    ListOfPositions[i["issueId"]] = i
            if PositionData.get("mercadoCapitales"):
                for i in PositionData["mercadoCapitales"]:
                    ListOfPositions[i["issueId"]] =i
            if ListOfPositions.get(Ticker):
                Ticker_Dict = ListOfPositions[Ticker]
                return self.Tickerdata(Ticker_Dict)
            return None
        else:
            return None
    def GetAllOrders(self):
        WebServiceURL = "https://homebroker-api.gbm.com/GBMP/api/Operation/GetBlotterByInstrument"
        now_utc = datetime.now(pytz.timezone('US/Central'))
        HourMinute = now_utc.strftime('%H:%M:%S')
        ProcesDate = now_utc.strftime('%a')
        ProcesDate += " "
        ProcesDate +=now_utc.strftime('%b')
        ProcesDate += " "
        ProcesDate +=now_utc.strftime('%Y-%m-%d').split("-")[2]
        ProcesDate += " "
        ProcesDate += HourMinute
        ProcesDate += " CST "
        ProcesDate += now_utc.strftime('%Y-%m-%d').split("-")[0]
        DataStream = {"contractId": self.ContractID,"instrumentTypes": [0, 2, 27, 28], "processDate": ProcesDate}
        Response = self.__GenerateRequest(WebServiceURL, DataStream, None)
        if Response:
            return Response
        else:
            return None
    def GetOrdersByState(self, State):
        AllTheOrdeers = self.GetAllOrders()
        if AllTheOrdeers is not None:
            OrderList = [x for x in AllTheOrdeers if x["orderStatus"] == State.value]
            return OrderList
        else:
            None
        #OrderState

    def GetTickerHistorical(self, Ticker, ElementsToTake):
        Uri = "https://homebroker-api.gbm.com/GBMP/api/Market/GetInstrumentPricesIntradayPPP/"
        #Uri += Ticker
        Uri += urllib.parse.quote(Ticker)
        DataStream = {"isOnLine": True}
        Response = self.__GenerateRequest(Uri, DataStream, None)
        if Response:
            LastNElements = Response[(-1 * ElementsToTake):]
            WeigthtsList = self.__GenerateWeigthsToMedian(ElementsToTake)
            WeigthedMedian = 0
            for i in range(0, len(WeigthtsList)):
                WeigthedMedian += WeigthtsList[i] * LastNElements[i]["price"]
            Data = self.HistoricalData()
            Data.IntradayPriceList = LastNElements

            Data.CurrentTicker = Response[-1]
            Data.CurrentPrice = Response[-1]["price"]
            Data.WeigthedMedian = WeigthedMedian
            Data.Median = sum(i["price"] for i in LastNElements) / ElementsToTake
            return Data
        else:
            return None

    def __GenerateWeigthsToMedian(self, iterations):
        ListOfWeigths =  list()
        denominator = 2.0
        for i in range(0, iterations):
            ListOfWeigths.append(1.0 / denominator)
            denominator *= 2
        ListOfWeigths.reverse()
        return ListOfWeigths
    class HistoricalData():
        def __init__(self):
            self.IntradayPriceList = dict()
            self.CurrentTicker = dict()
            self.Median = 0
            self.WeigthedMedian = 0
            self.CurrentPrice = 0
    def __Generate_GET_Request(self,WebServiceURL, HeadersDict ) -> dict:
        if HeadersDict == None:
            HeadersDict = dict()
            HeadersDict["Authorization"] = "Bearer " + self.BearerKey
            HeadersDict["Accept-Encoding"] = "identity"
            HeadersDict["Device-Type"] = "Mi A2 Lite"
            HeadersDict["Os-version"] = "Android 29"
            HeadersDict["App-version"] = "60"
            HeadersDict["Platform"] = "android"
            HeadersDict["User-agent"] = "okhttp/3.10.0"
            # HeadersDict["Accept"] = "application/json"
            HeadersDict["Content-Type"] = "application/json"
        response = requests.get(WebServiceURL, headers=HeadersDict)
        if(response.status_code == 200):
            JResponse = response.json()
            return JResponse
        return dict()
    def __GenerateRequest(self,WebServiceURL, Body, HeadersDict ) -> dict:
        if HeadersDict == None:
            HeadersDict = dict()
            HeadersDict["Authorization"] = "Bearer " + self.BearerKey
            HeadersDict["Accept-Encoding"] = "identity"
            HeadersDict["Device-Type"] = "Mi A2 Lite"
            HeadersDict["Os-version"] = "Android 29"
            HeadersDict["App-version"] = "60"
            HeadersDict["Platform"] = "android"
            HeadersDict["User-agent"] = "okhttp/3.10.0"
            # HeadersDict["Accept"] = "application/json"
            HeadersDict["Content-Type"] = "application/json"
        response = requests.post(WebServiceURL, json=Body, headers=HeadersDict)
        if(response.status_code == 200) or (response.status_code == 201):
            JResponse = response.json()
            return JResponse
        return dict()
    class MarketMovers():
        def __init__(self, winners, lossers, movers):
            winersList = list()
            for i in winners:
                winersList.append(GBM.StockData(i))
            lossersList = list()
            for i in lossers:
                lossersList.append(GBM.StockData(i))
            moversList = list()
            for i in movers:
                moversList.append(GBM.StockData(i))
            self.MarketRising = winersList
            self.MarketFalling = lossersList
            self.MarketVolume = moversList
    class contractBuyingPower():
        def __init__(self, ContactDict):
            self.buyingPower = ContactDict["buyingPower"]
            self.marketValueTotal = ContactDict["marketValueTotal"]
            self.totalCash = ContactDict["totalCash"]
            self.tradeMargin = ContactDict["tradeMargin"]
            self.pendingOrdersRisk = ContactDict["pendingOrdersRisk"]
            self.virtualGBMF2 = ContactDict["virtualGBMF2"]
            self.reporto = ContactDict["reporto"]
    class PurchaseOrderObject():
        def __init__(self, Contract, Ticker, Quantity, orderTypes, Price, instrumentType):
            self.contractId = Contract
            self.duration = "1"
            orderListing = list()
            Order = self.OrderObject(Quantity,orderTypes.value, Ticker, Price,instrumentType.value )
            Order.hash = self.__GenerateHash(Contract,Ticker,Quantity,orderTypes )
            orderListing.append(vars(Order))
            self.orders = orderListing

        def __GenerateHash(self, Contract, Ticker, Quant, TypeId):
            TheWild70s = datetime(year=1970, day=1, month=1)
            Now = datetime.now(timezone.utc)
            Now = Now.replace(tzinfo=None)
            Millis = int((Now - TheWild70s).total_seconds() * 1000)
            TickerName = Ticker.replace(" ", "")
            return str(Millis) + Contract + TickerName + str(Quant) + str((TypeId.value))
        class OrderObject():
            def __init__(self, quantity, orderTypes, Ticker, price, instrumentType=0):
                self.quantity = quantity
                self.capitalOrderTypeId = orderTypes
                self.issueId = Ticker
                self.hash = ""
                self.price = price
                self.instrumentType = instrumentType
                self.algoTradingTypeId = 0
    class OrderResponse():
        def __init__(self, OrderResponseDict):
            self.electronicOrderId = OrderResponseDict["electronicOrderId"]
            self.predespachadorId = OrderResponseDict["predespachadorId"]
            self.vigenciaId = OrderResponseDict["vigenciaId"]
            self.businessError = self.BussinessError(OrderResponseDict["businessError"]) if OrderResponseDict.get(
                "businessError") else "None"

        class BussinessError():
            def __init__(self, BussinerDict):
                self.errorCode = BussinerDict["errorCode"]
                self.errorMessage = BussinerDict["errorMessage"]
    class Tickerdata():
        def __init__(self, TickerDict):
            self.positionValueType = TickerDict["positionValueType"]
            self.issueId = TickerDict["issueId"]
            self.issueName = TickerDict["issueName"] if TickerDict.get("issueName") else "None"
            self.instrumentType = TickerDict["instrumentType"]
            self.quantity = TickerDict["quantity"]
            self.averagePrice = TickerDict["averagePrice"]
            self.lastPrice = TickerDict["lastPrice"]
            self.closePrice = TickerDict["closePrice"]
            self.weightedAveragePrice = TickerDict["weightedAveragePrice"]
            self.yieldValue = TickerDict["yieldValue"]
            self.marketValue = TickerDict["marketValue"]
            self.dailyVariationPercentage = TickerDict["dailyVariationPercentage"]
            self.historicalVariationPercentage = TickerDict["historicalVariationPercentage"]
            self.averageCost = TickerDict["averageCost"]
            self.positionPercentage = TickerDict["positionPercentage"]
    class StockData():
        def __init__(self, StockDict):
            self.issueId = StockDict["issueId"]
            self.openPrice = StockDict["openPrice"]
            self.maxPrice = StockDict["maxPrice"]
            self.minPrice = StockDict["minPrice"]
            self.percentageChange = StockDict["percentageChange"]
            self.valueChange = StockDict["valueChange"]
            self.aggregatedVolume = StockDict["aggregatedVolume"]
            self.bidPrice = StockDict["bidPrice"]
            self.bidVolume = StockDict["bidVolume"]
            self.askPrice = StockDict["askPrice"]
            self.askVolume = StockDict["askVolume"]
            self.lastPrice = StockDict["lastPrice"]
            self.closePrice = StockDict["closePrice"]
            self.instrumentTypeId = StockDict["instrumentTypeId"]
            self.benchmarkId = StockDict["benchmarkId"]
            self.riseLowTypeId = StockDict["riseLowTypeId"]
            self.benchmarkPercentage = StockDict["benchmarkPercentage"]
            self.benchmarkName = StockDict["benchmarkName"] if StockDict.get("benchmarkName") else "None"
            self.ipcParticipationRate = StockDict["ipcParticipationRate"] if StockDict.get("ipcParticipationRate") else "None"



    class OrderTypes(Enum):
        Sell = 8
        Buy = 1
    class InstrumentTypes(Enum):
        SIC = 0
        IPC = 2
    class OrderState(Enum):
        Done = 7
        Canceled = 5
        Rejected = 9
        Pending = 2