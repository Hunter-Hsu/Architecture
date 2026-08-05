import { Component } from "@angular/core";
import AppInputComponent from "./input.component";

@Component({
    selector: "app-input-text",
    templateUrl: "./input.component.html"
})
export default class AppInputTextComponent extends AppInputComponent {
    constructor() {
        super("text");
    }
}
