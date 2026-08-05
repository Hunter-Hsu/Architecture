import { Component, inject, signal } from "@angular/core";
import { FormField, FormRoot, form as createForm, required } from "@angular/forms/signals";
import AppAuthService from "@services/auth.service";
import AppButtonComponent from "@components/button/button.component";
import AppInputPasswordComponent from "@components/input/password.input.component";
import AppInputTextComponent from "@components/input/text.input.component";
import AppLabelComponent from "@components/label/label.component";
import AppAuthRequest from "@app/dtos/auth.request";

@Component({
    selector: "app-auth",
    templateUrl: "./auth.component.html",
    imports: [
        FormField,
        FormRoot,
        AppButtonComponent,
        AppInputPasswordComponent,
        AppInputTextComponent,
        AppLabelComponent
    ]
})
export default class AppAuthComponent {
    private readonly appAuthService = inject(AppAuthService);
    private readonly authRequest = signal<AppAuthRequest>({ login: "admin", password: "admin" });

    readonly form = createForm(
        this.authRequest,
        (path) => {
            required(path.login);
            required(path.password);
        },
        {
            submission: {
                action: async (field) => {
                    await this.appAuthService.signin(field().value());
                }
            }
        }
    );
}
