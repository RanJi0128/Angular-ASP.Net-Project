import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

const URI = {
  weatherForecasts: 'api/SampleData/WeatherForecasts',
  weatherSummaries: 'api/SampleData/GetSummaries'
}

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})

export class FetchDataComponent {
  public forecasts: WeatherForecast[];
  public allForecasts: WeatherForecast[];
  public summaries: Summary[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + URI.weatherForecasts).subscribe(result => {
      this.forecasts = result;
      this.allForecasts = result;
    }, error => console.error(error));

    http.get<Summary[]>(baseUrl + URI.weatherSummaries).subscribe(result => {
      this.summaries = result;
    }, error => console.error(error));
  }

  filterForecasts(filterVal: any) {
    this.forecasts = (filterVal == "0") ? this.allForecasts : this.allForecasts.filter((item) => item.summary == filterVal);
  }

}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

interface Summary {
  name: string;
}
