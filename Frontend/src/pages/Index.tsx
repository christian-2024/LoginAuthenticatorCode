import { useEffect, useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { useToast } from "@/hooks/use-toast";
import { Eye, EyeOff, LogIn } from "lucide-react";

const formSchema = z.object({
  email: z.string().min(1, "Informe seu e-mail").email("E-mail inválido"),
  password: z.string().min(6, "Mín. 6 caracteres"),
  remember: z.boolean().optional().default(false),
});

type FormValues = z.infer<typeof formSchema>;

const Index = () => {
  const { toast } = useToast();
  const [showPassword, setShowPassword] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: { email: "", password: "", remember: false },
    mode: "onChange",
  });

  useEffect(() => {
    document.title = "Login — Spark App";
    const metaDesc = document.querySelector('meta[name="description"]');
    if (metaDesc) metaDesc.setAttribute("content", "Tela de login moderna e responsiva com validação no frontend.");
    let canonical = document.querySelector('link[rel="canonical"]') as HTMLLinkElement | null;
    if (!canonical) {
      canonical = document.createElement('link');
      canonical.setAttribute('rel', 'canonical');
      document.head.appendChild(canonical);
    }
    canonical!.setAttribute('href', window.location.origin + '/');
  }, []);

  const onMouseMove = (e: React.MouseEvent<HTMLDivElement>) => {
    const el = containerRef.current;
    if (!el) return;
    const rect = el.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;
    el.style.setProperty('--x', `${x}px`);
    el.style.setProperty('--y', `${y}px`);
  };

  const onSubmit = async (data: FormValues) => {
    toast({ title: "Processando login...", description: "Validando credenciais" });
    await new Promise((r) => setTimeout(r, 800));
    toast({ title: "Login realizado!", description: `Bem-vindo, ${data.email}` });
  };

  const {
    register,
    handleSubmit,
    formState: { errors, isValid, isSubmitting },
  } = form;

  return (
    <main
      ref={containerRef}
      onMouseMove={onMouseMove}
      className="min-h-screen grid lg:grid-cols-2 relative"
      aria-labelledby="page-title"
    >
      <div className="absolute inset-0 bg-auth-gradient opacity-20" aria-hidden="true" />
      <div className="pointer-glow" aria-hidden="true" />

      <section className="flex items-center justify-center p-6 sm:p-8">
        <article className="w-full max-w-md glass-surface rounded-lg p-8">
          <header className="mb-6">
            <h1 id="page-title" className="text-3xl font-semibold tracking-tight">Login</h1>
            <p className="text-sm text-muted-foreground mt-1">Acesse sua conta para continuar</p>
          </header>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-5" noValidate>
            <div className="space-y-2">
              <Label htmlFor="email">E-mail</Label>
              <Input id="email" type="email" placeholder="voce@exemplo.com" autoComplete="email" {...register("email")} />
              {errors.email && (
                <p className="text-sm text-destructive">{errors.email.message}</p>
              )}
            </div>

            <div className="space-y-2">
              <div className="flex items-center justify-between">
                <Label htmlFor="password">Senha</Label>
                <button
                  type="button"
                  onClick={() => setShowPassword((v) => !v)}
                  className="text-sm text-muted-foreground hover:text-foreground transition-colors"
                  aria-label={showPassword ? "Ocultar senha" : "Mostrar senha"}
                >
                  {showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
                </button>
              </div>
              <div className="relative">
                <Input
                  id="password"
                  type={showPassword ? "text" : "password"}
                  placeholder="Sua senha"
                  autoComplete="current-password"
                  {...register("password")}
                />
              </div>
              {errors.password && (
                <p className="text-sm text-destructive">{errors.password.message}</p>
              )}
            </div>

            <div className="flex items-center justify-between">
              <label className="inline-flex items-center gap-2 text-sm text-muted-foreground">
                <Checkbox id="remember" {...register("remember")} />
                Lembrar-me
              </label>
              <a href="#" className="text-sm text-primary hover:underline">Esqueceu a senha?</a>
            </div>

            <Button type="submit" disabled={!isValid || isSubmitting} className="w-full">
              <LogIn className="mr-2 h-4 w-4" /> Entrar
            </Button>

            <p className="text-sm text-muted-foreground text-center">
              Novo por aqui? <a href="#" className="text-primary hover:underline">Criar conta</a>
            </p>
          </form>
        </article>
      </section>

      <aside className="hidden lg:flex items-center justify-center p-8 relative overflow-hidden">
        <div className="max-w-lg text-center">
          <h2 className="text-3xl font-semibold">Bem-vindo de volta</h2>
          <p className="mt-2 text-muted-foreground">Uma experiência de autenticação elegante, rápida e segura.</p>
        </div>
      </aside>
    </main>
  );
};

export default Index;
