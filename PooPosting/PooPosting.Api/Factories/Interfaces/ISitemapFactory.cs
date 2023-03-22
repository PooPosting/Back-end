﻿using X.Web.Sitemap;

namespace PooPosting.Api.Factories.Interfaces;

public interface ISitemapFactory
{
    Task<Sitemap> GenerateSitemap();
}