import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'dateAgo'
})
export class DateAgoPipe implements PipeTransform {
  divider = [60, 60, 24, 30, 12];
  string = [' sekund', ' minut', ' godzin', ' d', ' miesi', ' '];

  transform(value: any, ...args: unknown[]): unknown {
    if (!value) { return 'jakiś czas temu'; }
    let time = (Date.now() - Date.parse(value)) / 1000;
    if (time < 60) {
      return 'przed chwilą';
    }
    let i;
    for (i = 0; Math.floor(time / this.divider[i]) > 0; i++) {
      time /= this.divider[i];
    }
    const plural = this.generatePlPlural(time, i);
    return Math.floor(time) + plural + ' temu';
  }


  private generatePlPlural(time: number, timestampIndex: number) {
    switch (timestampIndex) {
      case (0): {
        if(Math.floor(time) === 1) return `${this.string[0]}ę`;
        if(this.strangePolishRulesCheck(time)) return `${this.string[0]}y`;
        return `${this.string[0]}`;
      }
      case (1): {
        if(Math.floor(time) === 1) return `${this.string[1]}ę`;
        if(this.strangePolishRulesCheck(time)) return `${this.string[1]}y`;
        return `${this.string[1]}`;
      }
      case (2): {
        if(Math.floor(time) === 1) return `${this.string[2]}ę`;
        if(this.strangePolishRulesCheck(time)) return `${this.string[2]}y`;
        return `${this.string[2]}`;
      }
      case (3): {
        if(Math.floor(time) === 1) return `${this.string[3]}zień`;
        return `${this.string[3]}ni`;
      }
      case (4): {
        if(Math.floor(time) === 1) return `${this.string[4]}ąc`;
        if (this.strangePolishRulesCheck(time)) return `${this.string[4]}ące`;
        return `${this.string[4]}ęcy`;
      }
      case (5): {
        if(Math.floor(time) === 1) return `${this.string[5]}rok`;
        if (this.strangePolishRulesCheck(time)) return `${this.string[5]}lata`;
        return `${this.string[5]}lat`;
      }
      default: {
        return "dawno";
      }
    }
  }

  strangePolishRulesCheck(time: number): boolean {
    return ((Math.floor(time) >= 2 && Math.floor(time) <= 4) ||
        (Math.floor(time) % 10 === 2 || Math.floor(time) % 10 === 3 ||
        Math.floor(time) % 10 === 4) && (Math.floor(time) >= 20));
  }

}
