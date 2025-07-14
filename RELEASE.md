# Release Process Guide

Bu kılavuz, Fermion.EntityFramework.Shared paketinin release sürecini açıklar.

## 🚀 Hızlı Başlangıç

### 1. Manuel Release (Yerel)

```bash
# Patch release (örnek: 1.0.1 -> 1.0.2)
make release
# veya
make release-patch

# Minor release (örnek: 1.0.1 -> 1.1.0)
make release-minor

# Major release (örnek: 1.0.1 -> 2.0.0)
make release-major
```

### 2. GitHub Actions ile Release

1. **GitHub Repository'de Actions sekmesine gidin**
2. **"Publish Package" workflow'unu seçin**
3. **"Run workflow" butonuna tıklayın**
4. **Version type'ı seçin (patch, minor, major)**
5. **"Run workflow" ile başlatın**

## 📋 Release Süreci

### Otomatik Adımlar:

1. **Version Belirleme**: En son git tag'ini alır ve belirtilen türde artırır
2. **Proje Güncelleme**: `.csproj` dosyasındaki `<Version>` değerini günceller
3. **Build & Pack**: Projeyi build eder ve NuGet paketi oluşturur
4. **Git İşlemleri**: 
   - Değişiklikleri commit eder
   - Yeni version ile git tag oluşturur
   - Changes'i GitHub'a push eder
5. **GitHub Actions Tetikleme**: Tag push edildiğinde otomatik olarak publish işlemi başlar
6. **NuGet Yayınlama**: Paketi NuGet.org'a yayınlar
7. **GitHub Release**: GitHub'da release oluşturur

## 🔧 Kurulum

### Gereksinimler:

1. **NuGet API Key**: `NUGET_API_KEY` secret'ını GitHub repository'nize ekleyin
   - NuGet.org hesabınızdan API key oluşturun
   - GitHub repo → Settings → Secrets and variables → Actions
   - `NUGET_API_KEY` adında secret oluşturun

2. **Git Yapılandırması**: Yerel kullanım için git config'i kontrol edin
   ```bash
   git config --global user.name "Your Name"
   git config --global user.email "your.email@example.com"
   ```

### İlk Kurulum:

```bash
# Script'leri executable yapın
chmod +x scripts/release.sh

# İlk tag'i oluşturun (eğer yoksa)
git tag v1.0.0
git push origin v1.0.0
```

## 📝 Version Türleri

- **Patch** (1.0.1 → 1.0.2): Bug fixes, küçük değişiklikler
- **Minor** (1.0.1 → 1.1.0): Yeni özellikler, backward compatible
- **Major** (1.0.1 → 2.0.0): Breaking changes, major değişiklikler

## 🔄 Workflow Detayları

### Manuel Tetikleme:
```
GitHub Actions → Publish Package → Run workflow
```

### Otomatik Tetikleme:
```
git tag v1.0.2 → GitHub Actions → NuGet Publish
```

## 📊 Makefile Komutları

```bash
make help           # Kullanılabilir komutları göster
make release        # Patch release oluştur
make release-patch  # Patch release oluştur
make release-minor  # Minor release oluştur
make release-major  # Major release oluştur
make build          # Projeyi build et
make test           # Testleri çalıştır
make clean          # Build artifacts'ları temizle
```

## 🐛 Sorun Giderme

### Script çalışmıyor:
```bash
chmod +x scripts/release.sh
```

### Git tag problemi:
```bash
# Mevcut tag'leri listele
git tag -l

# Tag sil (yanlış tag oluşturduysanız)
git tag -d v1.0.1
git push origin :refs/tags/v1.0.1
```

### NuGet publish hatası:
- `NUGET_API_KEY` secret'ının doğru olduğundan emin olun
- NuGet.org'da aynı version'ın zaten var olup olmadığını kontrol edin

## 📈 Örnek Kullanım

```bash
# Şu anki version: v1.0.1
# Patch release yapmak istiyorsanız:
make release

# Bu işlem:
# 1. v1.0.2 olarak version'ı artırır
# 2. .csproj dosyasını günceller
# 3. Paketi oluşturur
# 4. Git'e commit/tag/push eder
# 5. GitHub Actions tetiklenir
# 6. NuGet'e yayınlanır
```

## 🚨 Dikkat Edilmesi Gerekenler

1. **Main branch'te çalışın**: Release işlemi main branch'te yapılmalı
2. **Temiz working directory**: Commit edilmemiş değişiklikler olmamalı
3. **Test edilmiş kod**: Release öncesi testlerin geçtiğinden emin olun
4. **Anlamlı commit messages**: Semantic versioning'e uygun commit mesajları kullanın 